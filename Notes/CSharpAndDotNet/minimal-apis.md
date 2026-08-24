# Minimal APIs

Minimal APIs are a lightweight way to build HTTP APIs in ASP.NET Core with minimal ceremony —
no controllers, no attributes-heavy scaffolding. They're great for small services,
microservices, and learning the request pipeline.

> Related: see [request-validation.md](request-validation.md) for validating Minimal API
> requests, and [netcore-basics.md](netcore-basics.md) for middleware and DI.

## Basic Endpoints

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/users/{id}", (int id) => $"User {id}");
app.MapPost("/users", (CreateUserRequest request) => Results.Created($"/users/{request.Id}", request));

app.Run();
```

## Routing & Binding

Route parameters and query string values are bound automatically. You can bind from the body
by declaring a complex type parameter.

```csharp
app.MapGet("/products", (int page = 1, int size = 20) => $"Page {page}, Size {size}");
app.MapGet("/products/{id}", (int id) => $"Product {id}");
app.MapPost("/products", (Product product) => Results.Ok(product)); // body binding
```

## `MapGroup`

`MapGroup` groups endpoints that share a common prefix and lets you apply filters/metadata to
the whole group.

```csharp
var users = app.MapGroup("/users");

users.MapGet("/", () => "All users");
users.MapGet("/{id:int}", (int id) => $"User {id}");
users.MapPost("/", (CreateUserRequest request) => Results.Created());
```

## Returning Results

Use the `Results` static factory (or typed `TypedResults`) for well-described responses.

```csharp
app.MapGet("/items/{id}", (int id) =>
    id > 0
        ? Results.Ok(new Item(id))
        : Results.NotFound());

app.MapPost("/items", (Item item) =>
    Results.Created($"/items/{item.Id}", item));
```

Common results:

| Factory | Status code | Use |
| --- | --- | --- |
| `Results.Ok` | 200 | Success with body |
| `Results.Created` | 201 | Resource created |
| `Results.NoContent` | 204 | Success, no body |
| `Results.BadRequest` | 400 | Invalid request |
| `Results.NotFound` | 404 | Resource missing |
| `Results.ValidationProblem` | 400 | Validation errors |
| `Results.Unauthorized` | 401 | Not authenticated |

## Endpoint Filters

`IEndpointFilter` lets you run logic before/after an endpoint — a natural home for validation,
logging, or auth. Filters can be applied per endpoint or to a group and are composable.

```csharp
app.MapPost("/users", (CreateUserRequest request) => Results.Created())
    .AddEndpointFilter<ValidationFilter<CreateUserRequest>>();
```

See [request-validation.md](request-validation.md) for a full `ValidationFilter<T>` example.

## OpenAPI / Endpoint Metadata

Minimal APIs can document themselves with OpenAPI. Add `WithName`/`WithOpenApi` metadata and
use `TypedResults` so the generated schema is accurate.

```csharp
builder.Services.AddOpenApi();
app.MapOpenApi();
```

## Minimal API vs Controllers

| Aspect | Minimal APIs | Controllers |
| --- | --- | --- |
| Ceremony | Low | Higher (attributes, base class) |
| Body validation | Endpoint filters / built-in | Automatic via model binding |
| Structure | Good for small services | Good for large, layered apps |
| Learning curve | Shallow | Steeper |

## Key Takeaways

- Minimal APIs are concise and great for small services.
- Use route/query/body binding and `Results` for typed responses.
- `MapGroup` keeps related endpoints organized.
- `IEndpointFilter` is the composable way to add cross-cutting concerns (validation, auth).
- Add OpenAPI metadata so the API self-documents.
