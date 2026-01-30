# Lab Manual: AuthApi (Identity Provider)

This guide covers setting up the **Identity Provider** service using ASP.NET Core Identity.

## Goal
Create an API that manages users, handles logins, and issues JWTs (JSON Web Tokens).

## Step 1: Project Setup
1.  **Create Project:**
    ```bash
    dotnet new webapi -n AuthApi --no-openapi
    cd AuthApi
    ```
2.  **Install Dependencies:**
    ```bash
    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    dotnet add package Microsoft.EntityFrameworkCore.Sqlite
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```

## Step 2: Database Configuration
1.  **Create `Data/ApplicationDbContext.cs`:**
    *   Inherit from `IdentityDbContext<IdentityUser>`.
    *   This provides tables for Users, Roles, Claims, etc.
2.  **Configure `Program.cs`:**
    *   Add `AddDbContext` with SQLite.
    *   Add `AddIdentityCore<IdentityUser>` with EntityFrameworkStores.
3.  **Migration Tooling:**
    *   We use a local tool manifest (`dotnet-tools.json`) to manage `dotnet-ef`.
    *   See `dotnet-tools.json` for details.
    *   Run `dotnet dotnet-ef migrations add InitialCreate` to generate schema.
4.  **CORS:**
    *   Add `builder.Services.AddCors(...)` to allow cross-origin requests.
    *   Use `app.UseCors("AllowAll")` before Auth middlewares.

## Step 3: Authentication Logic (`AuthController`)
We use `UserManager<IdentityUser>` for all logic.

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuthApi.Models;

namespace AuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthRequest request)
    {
        var user = new IdentityUser { UserName = request.Username, Email = request.Username };
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return Ok(new { user.Id, user.UserName });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            return BadRequest("Wrong password.");
        }

        string token = CreateToken(user);
        return Ok(token);
    }

    private string CreateToken(IdentityUser user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("JwtSettings:Key").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}
```

## Step 4: Configuration (`appsettings.json`)
```json
"JwtSettings": {
  "Key": "super-secret-key-that-should-be-stored-securely-and-is-long-enough-for-hs512",
  "Issuer": "http://localhost:5248",
  "Audience": "http://localhost:5248"
}
```

## Step 5: Seeding
In `Program.cs`, we added a check after `EnsureDatabaseCreated`:
```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuthApi.Data.ApplicationDbContext>();
    context.Database.EnsureCreated();

    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>();
    if (await userManager.FindByNameAsync("student@example.com") == null)
    {
        var user = new Microsoft.AspNetCore.Identity.IdentityUser
        {
            UserName = "student@example.com",
            Email = "student@example.com"
        };
        await userManager.CreateAsync(user, "P@ssw0rd!");
    }
}
```

## How to Run
```bash
dotnet run
```
The app will start (usually port 5000-5200) and automatically seed the database.
