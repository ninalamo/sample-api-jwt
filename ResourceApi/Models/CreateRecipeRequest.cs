namespace ResourceApi.Models;

public record CreateRecipeRequest(string Title, string Instructions, List<IngredientDto> Ingredients);
