namespace ResourceApi.Models;

public class Recipe
{
    public string Title { get; set; } = string.Empty;
    public List<Ingredient> Ingredients { get; set; } = new();
    public string Instruction { get; set; } = string.Empty;
}

public class Ingredient
{
    public string Uom { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string Item { get; set; } = string.Empty;
}
