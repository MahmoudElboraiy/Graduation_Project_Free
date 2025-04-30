using AutoMapper;
using DataAcess;
using Domain.DTOs.Food;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        //private readonly ConvertExcelDbContext _db;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public FoodController( IMapper mapper, ApplicationDbContext db)
        {
            _db = db;
            _mapper = mapper;
        }
        //[Authorize]
        [HttpGet]
        public IActionResult getALL()
        {
            
            var r = _db.Ingredient.ToList();
            return Ok(r);
        }
        [HttpGet("{Name:alpha}")]
        public IActionResult GetSearchByName(string Name)
        {
            var recipes = _db.Recipe.Where(x => EF.Functions
            .Like(x.Recipe_Name, $"%{Name}%"))
                .Take(50)
                .AsNoTracking()
                .ToList();

            //if (pageSize > 0)
            //{
            //    if (pageSize > 100)
            //    {
            //        pageSize = 100;
            //    }
            //    recipes = recipes.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            //}

            foreach (var recipe in recipes)
            {

                _db.Entry(recipe).Reference(r => r.Nutrition).Load();

                _db.Entry(recipe).Collection(r => r.Recipe_Ingredient).Load();

                foreach (var ri in recipe.Recipe_Ingredient)
                {
                    _db.Entry(ri).Reference(r => r.Ingredient).Load();
                }
            }
           
            //var recipes = _db.Recipe
            //    .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{Name}%"))
            //    .Include(r => r.Nutrition)
            //    .Include(r => r.Recipe_Ingredient)
            //        .ThenInclude(ri => ri.Ingredient)
            //    .AsNoTracking()
            //    .ToList();
            List<RecipeWithNutritionDTO> recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }
        [HttpGet("SearchByIngredient/{ingredientName:alpha}")]
        public IActionResult GetSearchByIngredient(string ingredientName)
        {
            ingredientName = ingredientName.ToLowerInvariant();

            var ingredientList = _db.Ingredient.AsNoTracking()
                .Where(ing => EF.Functions.Like(ing.Ingredient_Name, $"%{ingredientName}%")).AsNoTracking()
                .Select(i => i.Ingredient_Id);

            return Ok(ingredientList.ToList());
        }

        [HttpPost]
        public ActionResult<List<RecipeWithNutritionDTO>> PostSearchByIngredientId([FromBody] List<int> ingredientsId)
        {

            //var recipes = _db.Recipe_Ingredient.AsNoTracking()
            //       .Where(ri => ingredientsId.Contains(ri.Ingredient_Id))
            //       .Select(ri => ri.Recipe)
            //        .Distinct();
            var recipes = _db.Recipe_Ingredient
               .AsNoTracking()
               .Where(ri => ingredientsId.Contains(ri.Ingredient_Id))
               .Take(50)
               .Select(ri => ri.Recipe)
               .Distinct()
               .Include(r => r.Nutrition)
               .Include(r => r.Recipe_Ingredient)
                   .ThenInclude(ri => ri.Ingredient);

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
