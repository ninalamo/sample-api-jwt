namespace ResourceApi.Models;

public class RecipeDto
{
    public string Title { get; set; } = string.Empty;
    public List<IngredientDto> Ingredients { get; set; } = new();
    public string Instructions { get; set; } = string.Empty; // Changed from Instruction to Instructions to match React app
}

public class IngredientDto
{
    public string Item { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string Uom { get; set; } = string.Empty; // Added Uom to match frontend
}
