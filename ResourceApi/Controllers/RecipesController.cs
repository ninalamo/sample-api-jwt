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
        var entities = await context.Recipes.ToListAsync();

        // Select logic maps Entity -> DTO
        var dtos = entities.Select(entity => new RecipeDto
        {
            Title = entity.Title,
            Instructions = entity.Instruction, // Mapped to 'Instructions' for Frontend
            // Parse raw data into friendly format
            Ingredients = entity.RawIngredients.Split('|').Select(i =>
            {
                // Format assumed: Item,Quantity,Uom (e.g. Chicken,1,kg)
                // If old format (Item,Qty), default Uom to "unit"
                var parts = i.Split(',');
                return new IngredientDto
                {
                    Item = parts[0],
                    Quantity = parts[1],
                    Uom = parts.Length > 2 ? parts[2] : "unit"
                };
            }).ToList()
        });

        return Ok(dtos);
    }
}
