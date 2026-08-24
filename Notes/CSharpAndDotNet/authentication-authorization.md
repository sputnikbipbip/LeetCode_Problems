# Authentication & Authorization

Authentication and authorization are the two halves of access control in a web API. They are
often confused but are separate concerns:

- **Authentication** answers *"who are you?"* — it verifies identity (e.g. via a JWT).
- **Authorization** answers *"what are you allowed to do?"* — it enforces access rules based on
  that identity (roles, claims, policies).

> Related: validation ([request-validation.md](request-validation.md)) and Minimal APIs
> ([minimal-apis.md](minimal-apis.md)) combine with these to secure endpoints.

## Authentication vs Authorization

| | Authentication | Authorization |
| --- | --- | --- |
| Question | Who are you? | What can you do? |
| Order | Happens first | Happens after |
| Uses | Credentials / tokens | Roles / claims / policies |
| Failure result | `401 Unauthorized` | `403 Forbidden` |

## JWT Bearer Authentication

JWT (JSON Web Token) is the common stateless authentication mechanism for APIs and SPAs. The
client presents a signed token; the server validates it on each request.

### The flow

1. Client authenticates (login) and receives a signed JWT from the issuer.
2. The token contains **claims** (e.g. user id, roles) and an expiry.
3. Client calls the API with `Authorization: Bearer <token>`.
4. The API validates the token's **signature, issuer, audience, and expiry**.
5. ASP.NET Core builds `HttpContext.User` from the token's claims.
6. Authorization checks (policies/roles/claims) run before the endpoint executes.

### Registering JWT bearer authentication

```csharp
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        // or use explicit key/validation parameters
    });

builder.Services.AddAuthorization();
```

### Wiring the middleware

Order matters: `UseAuthentication` must come before `UseAuthorization`.

```csharp
var app = builder.Build();

app.UseAuthentication();   // establish identity from the token
app.UseAuthorization();    // enforce access rules

app.Run();
```

### Generating a token

```csharp
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, "Administrator")
};

var token = new JwtSecurityToken(
    issuer: config["Jwt:Issuer"],
    audience: config["Jwt:Audience"],
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(30),
    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
```

## Protecting Endpoints

### Controllers — `[Authorize]`

```csharp
[Authorize]                                  // any authenticated user
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts() => Ok();
}
```

### Minimal APIs

```csharp
app.MapGet("/admin", () => "Admin area")
    .RequireAuthorization();                  // any authenticated user

app.MapGet("/admin/delete", () => "Deleted")
    .RequireAuthorization("CanDelete");       // must satisfy a policy
```

## Authorization Models

### Role-based

Restrict access by role.

```csharp
[Authorize(Roles = "Administrator")]
public IActionResult DeleteProduct(int id) => Ok();
```

### Policy-based (recommended)

Policies are named rules composed of requirements/claims. They are more flexible than roles
and keep authorization logic centralized.

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagersOnly",
        policy => policy.RequireClaim("Department", "Management"));

    options.AddPolicy("CanDeleteProduct",
        policy => policy.RequireRole("Administrator"));
});
```

```csharp
[Authorize(Policy = "ManagersOnly")]
public IActionResult Reports() => Ok();
```

### Claims-based

Check for the presence (and value) of specific claims.

```csharp
options.AddPolicy("VerifiedEmail",
    policy => policy.RequireClaim(ClaimTypes.Email));
```

## 401 vs 403

| Status | Meaning |
| --- | --- |
| `401 Unauthorized` | You are **not authenticated** — no/invalid token. |
| `403 Forbidden` | You are **authenticated** but not allowed to access the resource. |

## Security Best Practices

- **Always validate** issuer, audience, expiration, and signature.
- Enforce **HTTPS** to prevent token/credential theft.
- Keep JWT payloads **small** — avoid storing too many claims.
- Use **short-lived access tokens** and consider **refresh tokens**.
- Rotate signing keys and store secrets securely (not in code).
- Use **least privilege** — specific roles/policies instead of a blanket "Admin".
- Protect every sensitive endpoint; don't rely on client-side checks.
- Never trust client-supplied authorization.

## Common Mistakes

- Hardcoding JWT secrets.
- Disabling token validation.
- Using long-lived access tokens.
- Logging sensitive authentication data.
- Forgetting to protect administrative endpoints.
- Treating frontend auth as backend security.

## Key Takeaways

- **Authentication** = identity; **Authorization** = access. They return different errors
  (401 vs 403).
- JWT bearer auth validates the token and populates `HttpContext.User` from claims.
- `UseAuthentication()` must run before `UseAuthorization()`.
- Prefer **policy-based** authorization over hardcoded role checks.
- Secure configuration and token hygiene are non-negotiable.
