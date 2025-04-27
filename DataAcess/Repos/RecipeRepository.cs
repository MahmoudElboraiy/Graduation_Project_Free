using DataAcess.Repos.IRepos;
using Domain.Domain;
using Domain.DTOs.Food;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(FullRecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                Recipe_Name = recipeDTO.Name,
                Time = recipeDTO.Time,
                Description = recipeDTO.Description,
                Preparation_Method = recipeDTO.Preparation_Method,
            };

            _db.Recipe.Add(recipe);
            await _db.SaveChangesAsync(); 

            var nutrition = new Nutrition
            {
                Recipe_Id = recipe.Recipe_Id,
                Calories_100g = recipeDTO.Calories_100g,
                Fat_100g = recipeDTO.Fat_100g,
                Sugar_100g = recipeDTO.Sugar_100g,
                Protein_100g = recipeDTO.Protein_100g,
                Carb_100 = recipeDTO.Carb_100,
                Type = recipeDTO.Type
            };
            _db.Nutrition.Add(nutrition);

            foreach (var ingredientDto in recipeDTO.ingredientDtos)
            {
                var ingredient = await _db.Ingredient
                    .FirstOrDefaultAsync(i => i.Ingredient_Name == ingredientDto.Name);

                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Ingredient_Name = ingredientDto.Name
                        
                    };
                    _db.Ingredient.Add(ingredient);
                    await _db.SaveChangesAsync(); 
                }

                var recipeIngredient = new Recipe_Ingredient
                {
                    RecipeId = recipe.Recipe_Id,
                    Ingredient_Id = ingredient.Ingredient_Id,
                    Amount = ingredientDto.Amount
                };
                _db.Recipe_Ingredient.Add(recipeIngredient);
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await _db.Recipe
       .Include(r => r.Recipe_Ingredient)
       .FirstOrDefaultAsync(r => r.Recipe_Id == id);

            if (recipe == null)
                throw new Exception("Recipe not found");

            var nutrition = await _db.Nutrition
                .FirstOrDefaultAsync(n => n.Recipe_Id == recipe.Recipe_Id);

            if (nutrition != null)
                _db.Nutrition.Remove(nutrition);

            _db.Recipe_Ingredient.RemoveRange(recipe.Recipe_Ingredient);
            _db.Recipe.Remove(recipe);

            await _db.SaveChangesAsync();
        }

        public Task<List<Recipe>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Recipe> GetAllQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetAllWithoutTrackingAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<FullRecipeDTO?> GetAsync(int id)
        {
            return await _db.Recipe
          .AsNoTracking()
          .Where(r => r.Recipe_Id == id)
          .Select(r => new FullRecipeDTO
          {
              Name = r.Recipe_Name,
              Time = r.Time,
              Description = r.Description,
              Preparation_Method = r.Preparation_Method,
              Type = r.Nutrition.Type,
              Calories_100g = r.Nutrition.Calories_100g,
              Fat_100g = r.Nutrition.Fat_100g,
              Sugar_100g = r.Nutrition.Sugar_100g,
              Protein_100g = r.Nutrition.Protein_100g,
              Carb_100 = r.Nutrition.Carb_100,
              ingredientDtos = r.Recipe_Ingredient
                  .Select(ri => new IngredientDto
                  {
                      Name = ri.Ingredient.Ingredient_Name,
                      Amount = (double)ri.Amount
                  }).ToList()
          })
          .FirstOrDefaultAsync();
        }

        public Task<Recipe?> GetWithoutTrackingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, FullRecipeDTO dto)
        {
            var recipe = await _db.Recipe
               .Include(r => r.Recipe_Ingredient)
               .FirstOrDefaultAsync(r => r.Recipe_Id == id);

            if (recipe == null)
                throw new Exception("Recipe not found");

            // Update basic info
            recipe.Recipe_Name = dto.Name;
            recipe.Description = dto.Description;
            recipe.Time = dto.Time;
            recipe.Preparation_Method = dto.Preparation_Method;

            // Update Nutrition
            var nutrition = await _db.Nutrition
                .FirstOrDefaultAsync(n => n.Recipe_Id == recipe.Recipe_Id);

            if (nutrition != null)
            {
                nutrition.Calories_100g = dto.Calories_100g;
                nutrition.Fat_100g = dto.Fat_100g;
                nutrition.Sugar_100g = dto.Sugar_100g;
                nutrition.Protein_100g = dto.Protein_100g;
                nutrition.Carb_100 = dto.Carb_100;
                nutrition.Type = dto.Type;
            }

            // Remove old ingredients
            _db.Recipe_Ingredient.RemoveRange(recipe.Recipe_Ingredient);

            // Add new ingredients
            foreach (var ingredientDto in dto.ingredientDtos)
            {
                var ingredient = await _db.Ingredient
                    .FirstOrDefaultAsync(i => i.Ingredient_Name == ingredientDto.Name);

                if (ingredient == null)
                {
                    ingredient = new Ingredient { Ingredient_Name = ingredientDto.Name };
                    _db.Ingredient.Add(ingredient);
                    await _db.SaveChangesAsync();
                }

                var recipeIngredient = new Recipe_Ingredient
                {
                    RecipeId = recipe.Recipe_Id,
                    Ingredient_Id = ingredient.Ingredient_Id,
                    Amount = ingredientDto.Amount
                };
                _db.Recipe_Ingredient.Add(recipeIngredient);
            }

            await _db.SaveChangesAsync();
        }

        public Task UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Recipe?> IRecipeRepository.GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
