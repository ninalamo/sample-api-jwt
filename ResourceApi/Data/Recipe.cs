using System.ComponentModel.DataAnnotations;

namespace ResourceApi.Data;

public class Recipe
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    // Internal secrets we NEVER want to show the public
    public string InternalComments { get; set; } = string.Empty;
    // Stored as a flat string (e.g. CSV) for efficiency in this legacy DB example
    public string RawIngredients { get; set; } = string.Empty;
    public string Instruction { get; set; } = string.Empty;
}
