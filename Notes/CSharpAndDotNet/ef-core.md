# Entity Framework Core

Entity Framework Core (EF Core) is the object-relational mapper (ORM) for .NET. It lets you
work with a database using C# objects and LINQ instead of writing SQL directly, while still
allowing raw SQL when needed.

> Related: EF Core `DbContext` is registered as **scoped** by default — see
> [netcore-basics.md](netcore-basics.md) for service lifetimes and
> [design-patterns.md](design-patterns.md) for the Singleton discussion.

## Setting Up a DbContext

A `DbContext` represents a session with the database. You define entity classes and expose
them as `DbSet<T>` properties.

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public List<Order> Orders { get; set; } = new();
}
```

### Registering with DI

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
```

For better performance under high concurrency, use pooling:

```csharp
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
```

## Migrations

Migrations track schema changes over time and let you apply them to the database.

```bash
dotnet ef migrations add InitialCreate   # create a migration from the model
dotnet ef database update                # apply migrations to the database
```

## Querying with LINQ to Entities

Queries are written with LINQ. EF Core translates them to SQL (deferred execution — the query
runs when enumerated).

```csharp
using var db = new AppDbContext(...);

// Filtering
var adults = db.Users.Where(u => u.Age >= 18);

// Projection
var names = db.Users.Select(u => new { u.Name, u.Email });

// Ordering
var sorted = db.Users.OrderBy(u => u.Name);

// Materialize the result
var list = await sorted.ToListAsync();
var first = await db.Users.FirstOrDefaultAsync(u => u.Email == "a@b.com");
```

### Tracking vs `AsNoTracking`

By default EF Core **tracks** entities it loads so you can save changes back. For read-only
queries, `AsNoTracking()` avoids that overhead.

```csharp
var users = await db.Users.AsNoTracking().ToListAsync();
```

## Adding / Updating / Deleting

```csharp
// Add
var user = new User { Name = "Alice", Email = "a@b.com" };
db.Users.Add(user);
await db.SaveChangesAsync();

// Update
user.Name = "Alicia";
await db.SaveChangesAsync(); // EF detects the change while tracked

// Delete
db.Users.Remove(user);
await db.SaveChangesAsync();
```

`SaveChangesAsync()` persists all pending changes in one transaction.

## Relationships

EF Core infers relationships from navigation properties.

### One-to-Many

```csharp
public class Order
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }   // foreign key
    public decimal Total { get; set; }
}
```

### Many-to-Many (implicit join)

```csharp
public class Product { public int Id { get; set; } public List<Order> Orders { get; set; } = new(); }
public class Order { public int Id { get; set; } public List<Product> Products { get; set; } = new(); }
```

Include related data with `Include` / `ThenInclude` (eager loading):

```csharp
var orders = await db.Orders
    .Include(o => o.User)
    .Include(o => o.Products)
    .ToListAsync();
```

## Async Best Practices

- Use the async variants: `ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`, `AnyAsync`.
- These are `Task`-based and won't block the thread — pair with `async`/`await`
  (see [async-programming.md](async-programming.md)).

## Key Takeaways

- `DbContext` is **scoped** and registered via DI; consider pooling for high concurrency.
- Migrations version your schema; `database update` applies them.
- LINQ to Entities translates to SQL with deferred execution.
- Use `AsNoTracking()` for read-only queries to reduce overhead.
- Express relationships with navigation properties and load them with `Include`.
- Always prefer async EF methods in web apps.
