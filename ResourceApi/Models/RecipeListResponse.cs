namespace ResourceApi.Models;

public record RecipeListResponse(int Id, string Title, string Instructions, List<IngredientDto> Ingredients);
