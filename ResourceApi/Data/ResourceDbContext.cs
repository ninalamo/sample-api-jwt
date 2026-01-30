using Microsoft.EntityFrameworkCore;

namespace ResourceApi.Data;

public class ResourceDbContext : DbContext
{
    public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recipe>().HasData(
            new Recipe
            {
                Id = 1,
                Title = "Chicken Adobo",
                InternalComments = "Grandma's secret recipe, do not share with competitors.",
                RawIngredients = "Chicken,1,kg|Soy Sauce,0.5,cup|Vinegar,0.5,cup|Garlic,1,head",
                Instruction = "Combine all ingredients in a pot. Marinate for 30 mins. Simmer until tender."
            }
        );
    }
}
