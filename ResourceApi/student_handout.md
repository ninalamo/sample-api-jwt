# Lab Manual: ResourceApi (Protected Service)

This guide covers setting up a **Protected API** that verifies JWT tokens.

## Goal
Create an API that serves data (Recipes) only to users with a valid JWT Token.

## Step 1: Project Setup
1.  **Create Project:**
    ```bash
    dotnet new webapi -n ResourceApi --no-openapi
    cd ResourceApi
    ```
2.  **Install Dependencies:**
    ```bash
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    ```
3.  **Configure CORS:**
    *   Enable CORS to allow browser clients to access this API.

## Step 2: Domain Model
1.  **Create `Models/Recipe.cs`:**
    *   Defines the data structure (Title, Ingredients, Instructions).

## Step 3: Protected Controller (`RecipesController`)
1.  **Create Controller:**
    *   Mark with `[Authorize]`.
    *   Return a list of Recipes.
2.  **Behavior:**
    *   If a request has no header -> **401 Unauthorized**.
    *   If a request has a valid header -> **200 OK** + Data.

## Step 4: Token Validation (`Program.cs`)
We must configure the app to understand how to validate the incoming token.
```csharp
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
```
```

## Step 5: Testing
1.  **Obtain a Token:** Get a valid JWT token from your Identity Provider.
2.  **Call API:**
    ```bash
    curl -H "Authorization: Bearer <TOKEN>" http://localhost:<PORT>/api/recipes
    ```
