using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceApi.Data;
using ResourceApi.Models;

namespace ResourceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipesController(ResourceDbContext context) : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // 1. Fetch from DB with Ingredients
        var entities = await context.Recipes
            .Include(r => r.Ingredients)
            .ToListAsync();

        // 2. Map Entity -> Response (Using Positional Records)
        var responses = entities.Select(e => new RecipeListResponse(
            e.Id,
            e.Title,
            e.Instruction,
            e.Ingredients.Select(i => new IngredientDto(
                i.Description,
                i.Quantity,
                i.UnitOfMeasure
            )).ToList()
        ));

        return Ok(responses);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRecipeRequest request)
    {
        var entity = new Recipe
        {
            Title = request.Title, // Accessing Record properties is the same!
            Instruction = request.Instructions,
            InternalComments = "Created via API",
            // Map the list directly!
            Ingredients = request.Ingredients.Select(i => new Ingredient
            {
                Description = i.Item,
                Quantity = i.Quantity,
                UnitOfMeasure = i.Uom
            }).ToList()
        };

        context.Recipes.Add(entity);
        await context.SaveChangesAsync();

        return Created("", new { Message = "Recipe Created!" });
    }
}
