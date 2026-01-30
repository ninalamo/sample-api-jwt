using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceApi.Models;

namespace ResourceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipesController : ControllerBase
{
    private static readonly List<Recipe> Recipes = new()
    {
        new Recipe
        {
            Title = "Chicken Adobo",
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Item = "Chicken", Quantity = 1, Uom = "kg" },
                new Ingredient { Item = "Soy Sauce", Quantity = 0.5, Uom = "cup" },
                new Ingredient { Item = "Vinegar", Quantity = 0.5, Uom = "cup" },
                new Ingredient { Item = "Garlic", Quantity = 1, Uom = "head" },
                new Ingredient { Item = "Bay Leaves", Quantity = 3, Uom = "pcs" },
                new Ingredient { Item = "Peppercorns", Quantity = 1, Uom = "tbsp" },
                new Ingredient { Item = "Salt", Quantity = 1, Uom = "tbsp" } // Required from user example
            },
            Instruction = "Combine all ingredients in a pot. Marinate for 30 mins. Simmer for 40 mins until tender. Fry slightly if desired."
        }
    };

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Recipes);
    }
}
