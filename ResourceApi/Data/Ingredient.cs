using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceApi.Data;

public class Ingredient
{
    [Key]
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;

    // Foreign Key
    public int RecipeId { get; set; }
}
