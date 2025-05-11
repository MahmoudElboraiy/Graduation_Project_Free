using AutoMapper;
using DataAcess;
using Domain.DTOs.Food;
using Domain.Interfaces;
using IdentityManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly ILanguageDbContext _db;
        private readonly IMapper _mapper;

        public FoodController(IMapper mapper, ILanguageDbContextAccessor dbAccessor, ILanguageDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }
        //[Authorize]
        [HttpGet]
        public IActionResult getALL(int? pageNumber = null, int? pageSize = null)
        {

            var data = _db.Recipe.AsNoTracking();
            if (pageNumber == null || pageNumber.Value <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                return BadRequest("Page size must be greater than zero.");
            }

            data = data
                    .OrderBy(r => r.Recipe_Id)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return Ok(data);
        }
        [HttpGet("{Name}")]
        public IActionResult GetSearchByName(string Name, int? pageNumber = null, int? pageSize = null)
        {

            var recipes = _db.Recipe
                   .Include(r => r.Nutrition)
                   .Include(r => r.Recipe_Ingredient)
                       .ThenInclude(ri => ri.Ingredient)
                   .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{Name}%"))
                      .AsNoTracking();
            if (pageNumber == null || pageNumber.Value <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                return BadRequest("Page size must be greater than zero.");
            }
            recipes = recipes
                    .OrderBy(r => r.Recipe_Id)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            List<RecipeWithNutritionDTO> recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }
        [HttpGet("SearchByIngredient/{ingredientName}")]
        public IActionResult GetSearchByIngredient(string ingredientName)
        {
            ingredientName = ingredientName.ToLowerInvariant();

            var ingredientList = _db.Ingredient.AsNoTracking()
                .Where(ing => EF.Functions.Like(ing.Ingredient_Name, $"%{ingredientName}%"))
                .Select(i => i.Ingredient_Id);

            return Ok(ingredientList.ToList());
        }

        [HttpPost]
        public ActionResult<List<RecipeWithNutritionDTO>> PostSearchByIngredientId([FromBody] List<int> ingredientsId, int? pageNumber = null, int? pageSize = null)
        {

            var recipes = _db.Recipe
                   .Where(r => r.Recipe_Ingredient.Any(ri => ingredientsId.Contains(ri.Ingredient_Id)))
                   .Include(r => r.Nutrition)
                   .Include(r => r.Recipe_Ingredient)
                       .ThenInclude(ri => ri.Ingredient)
                        .Distinct()
                        .AsNoTracking();

            if (pageNumber == null || pageNumber.Value <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                return BadRequest("Page size must be greater than zero.");
            }

            recipes = recipes
                    .OrderBy(r => r.Recipe_Id)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            List<RecipeWithNutritionDTO> recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }

        [HttpPost("MissingIngredients/{recipeId:int}")]
        public IActionResult GetMissingIngredients(int recipeId, [FromBody] List<int> availableIngredientIds)
        {
            var requiredIngredients = _db.Recipe_Ingredient
                .Where(ri => ri.RecipeId == recipeId)
                .Select(ri => ri.Ingredient_Id)
                .ToList();

            var missingIngredients = requiredIngredients
                .Where(ingredientId => !availableIngredientIds.Contains(ingredientId))
                .ToList();

            var missingIngredientDetails = _db.Ingredient
                .Where(ing => missingIngredients.Contains(ing.Ingredient_Id)
                         &&!ing.Type)
                .Select(ing => new
                {
                    ing.Ingredient_Name
                })
                .ToList();

            return Ok(missingIngredientDetails);
        }
    }
}
