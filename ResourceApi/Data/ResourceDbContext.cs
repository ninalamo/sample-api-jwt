using Microsoft.EntityFrameworkCore;
using ResourceApi.Data;

namespace ResourceApi.Data;

public class ResourceDbContext(DbContextOptions<ResourceDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; } = default!;
    public DbSet<Ingredient> Ingredients { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Recipe>().HasData(new Recipe
        {
            Id = 1,
            Title = "Chicken Adobo",
            InternalComments = "Grandma's secret",
            Instruction = "Combine and simmer."
        });

        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 1, Description = "Chicken", Quantity = "1", UnitOfMeasure = "kg", RecipeId = 1 },
            new Ingredient { Id = 2, Description = "Soy Sauce", Quantity = "0.5", UnitOfMeasure = "cup", RecipeId = 1 }
        );
    }
}
