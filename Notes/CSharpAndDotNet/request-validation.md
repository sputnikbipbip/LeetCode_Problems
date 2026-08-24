# DTO Request Validation

Validating incoming request bodies (DTOs) before they reach business logic is essential for
security (rejecting malformed/malicious input) and data integrity, and for returning a
**consistent, machine-readable error contract** to clients.

This note covers the four main techniques in ASP.NET Core:

1. **DataAnnotations** (attributes on the model)
2. **Custom Validation Attributes** (your own attributes / object-level rules)
3. **FluentValidation** (a library with fluent, testable rule classes)
4. **Minimal API Endpoint Filters** (`IEndpointFilter`)

It also covers the **built-in validation** added for Minimal APIs in .NET 10.

> Targeted at **.NET 10**. The built-in Minimal API validation is available since .NET 10.

## The Error Contract

All validation approaches should ultimately return a consistent structure. ASP.NET Core uses
`ValidationProblemDetails`, which is an RFC 7807 `ProblemDetails` object:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Email": ["'Email' must be a valid email address."],
    "Age": ["'Age' must be between 18 and 120."]
  }
}
```

Agree on this contract **before** choosing a technique — if different endpoints return
different error shapes, your clients break. This is the single most important cross-cutting
decision in validation.

## DataAnnotations

DataAnnotations use attributes on the DTO to declare structural rules. They work natively with
**controllers** (model binding validates automatically and returns `400` with
`ValidationProblemDetails`).

```csharp
public class CreateUserRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Range(18, 120)]
    public int Age { get; set; }

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = "";
}
```

Common attributes:

| Attribute | Purpose |
| --- | --- |
| `[Required]` | Value must not be `null`/empty. |
| `[StringLength(n)]` | Max (and optional min) length. |
| `[Range(min, max)]` | Numeric/date range. |
| `[EmailAddress]` | Email format check. |
| `[RegularExpression(pattern)]` | Custom pattern match. |
| `[Compare]` | Must equal another property. |
| `[MinLength]` / `[MaxLength]` | Collection/string bounds. |

You can also validate manually without the pipeline:

```csharp
var results = new List<ValidationResult>();
var isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);
```

**Trade-offs:**

- ✅ Zero setup with controllers; constraints reflected automatically in OpenAPI schema.
- ❌ Rules live on the model (coupling), hard to express cross-field/conditional/async rules,
  harder to unit-test in isolation.

## Custom Validation Attributes

When a single built-in attribute isn't enough, subclass `ValidationAttribute` and override
`IsValid`.

```csharp
public class MustBeInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is DateTime date && date <= DateTime.UtcNow)
        {
            return new ValidationResult("The date must be in the future.");
        }
        return ValidationResult.Success;
    }
}

public class CreateEventRequest
{
    [Required]
    public string Title { get; set; } = "";

    [MustBeInFuture]
    public DateTime StartsAt { get; set; }
}
```

### Object-level rules with `IValidatableObject`

For rules that span multiple properties, implement `IValidatableObject` on the DTO itself.

```csharp
public class BookingRequest : IValidatableObject
{
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (CheckOut <= CheckIn)
        {
            yield return new ValidationResult(
                "Check-out must be after check-in.",
                new[] { nameof(CheckOut) });
        }
    }
}
```

**Trade-offs:**

- ✅ Reusable, self-contained, still attribute-driven (OpenAPI-friendly where applicable).
- ❌ Still lives on the model; `IValidatableObject` gets unwieldy for complex logic.

## FluentValidation

FluentValidation keeps rules in **separate validator classes**, decoupled from the model. It
supports complex conditional rules, cross-field rules, and **async** validation (e.g. DB
uniqueness checks). Rules are plain classes, so they are easy to unit test.

```csharp
// Install: dotnet add package FluentValidation

public class CreateUserRequest
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public int Age { get; set; }
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Age).InclusiveBetween(18, 120);

        // Cross-field rule
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords must match.");

        // Async rule (e.g. uniqueness check against a DB)
        RuleFor(x => x.Email)
            .MustAsync(async (email, ct) => !await IsEmailTakenAsync(email, ct))
            .WithMessage("Email is already registered.");
    }
}
```

### Registering validators

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
```

### Validating manually

```csharp
public async Task ValidateExample(IValidator<CreateUserRequest> validator)
{
    var request = new CreateUserRequest { Name = "A" };
    ValidationResult result = await validator.ValidateAsync(request);

    if (!result.IsValid)
    {
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
        }
    }
}
```

**Trade-offs:**

- ✅ Testable in isolation, decoupled from models, async + cross-field capable, per-version
  validators.
- ❌ Extra dependency; OpenAPI doesn't reflect rules automatically (needs a bridge package).

## Minimal API Endpoint Filters

`IEndpointFilter` runs before/after an endpoint handler, making it a natural place to run
validation in Minimal APIs — without controllers.

```csharp
public class ValidationFilter<T> : IEndpointFilter
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator) => _validator = validator;

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();
        if (argument is not null)
        {
            var result = await _validator.ValidateAsync(argument);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(
                    result.ToDictionary(),
                    statusCode: StatusCodes.Status400BadRequest);
            }
        }
        return await next(context);
    }
}
```

Apply the filter per endpoint or to a whole group with `MapGroup`:

```csharp
var group = app.MapGroup("/users")
    .AddEndpointFilter<ValidationFilter<CreateUserRequest>>();

group.MapPost("/", (CreateUserRequest request) => Results.Created());
```

Filters are **composable** — you can stack them in order:

```csharp
app.MapPost("/orders", CreateOrder)
    .AddEndpointFilter<LoggingFilter>()
    .AddEndpointFilter<ValidationFilter<CreateOrderRequest>>()
    .AddEndpointFilter<AuthenticationFilter>();
```

**Trade-offs:**

- ✅ Composable, request-scoped enrichment possible, works for Minimal APIs.
- ❌ More plumbing; typically used together with FluentValidation.

## Built-in Validation for Minimal APIs (.NET 10)

.NET 10 added first-class validation for Minimal APIs via a **source generator**. You decorate
request types with DataAnnotations, register validation, and the framework returns `400
ValidationProblemDetails` automatically — with zero reflection and Native AOT support.

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. Register validation
builder.Services.AddValidation();

var app = builder.Build();

// 2. Decorate the request type with DataAnnotations
//    (e.g. CreateUserRequest with [Required], [EmailAddress], [Range], ...)

// 3. That's it — validation runs automatically before the handler
app.MapPost("/users", (CreateUserRequest request) => Results.Created());
```

When validation fails, the endpoint returns a `400` with `ValidationProblemDetails`.

> For .NET 11+, async DataAnnotations rules (`AsyncValidationAttribute`,
> `IAsyncValidatableObject`) are also available.

## Decision Matrix

| Scenario | Recommended approach |
| --- | --- |
| Simple structural rules (required, range, format) | DataAnnotations / built-in (.NET 10) |
| Complex, cross-field, or conditional rules | FluentValidation |
| Async rules that need I/O (e.g. DB checks) | FluentValidation (`MustAsync`) |
| Native AOT / zero dependencies | Built-in DataAnnotations (source-generated) |
| Request-scoped error enrichment (tenant, trace ID) | Custom endpoint filter |
| Testability / rule isolation | FluentValidation |

**In practice, most mature APIs use a hybrid:** built-in or DataAnnotations for the simple
DTOs, FluentValidation for the handful of request types with genuinely complex rules. The
discipline that keeps this working is a **single agreed error contract**.

## Testing Validators

Validation rules are behavior and should be tested. FluentValidation makes this
straightforward — instantiate the validator, call `Validate`/`ValidateAsync`, assert the
result — no hosting required.

```csharp
[Fact]
public void CreateUserRequestValidator_Rejects_ShortName()
{
    var validator = new CreateUserRequestValidator();
    var request = new CreateUserRequest { Name = "A" };

    var result = validator.Validate(request);

    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Name));
}
```

## Key Takeaways

- Agree on a **single error contract** (`ValidationProblemDetails`) before choosing a technique.
- **DataAnnotations**: fast, built-in, OpenAPI-friendly, but limited to structural rules.
- **Custom attributes / `IValidatableObject`**: reuse + object-level rules, still model-coupled.
- **FluentValidation**: decoupled, testable, async + cross-field — the default for complex rules.
- **Endpoint filters**: composable pipeline, ideal for Minimal APIs.
- **.NET 10 built-in validation**: zero-dependency, source-generated DataAnnotations for
  Minimal APIs.
- Use a **hybrid** approach; don't over-engineer a greenfield API with a library before you
  have a rule the framework can't already express.
