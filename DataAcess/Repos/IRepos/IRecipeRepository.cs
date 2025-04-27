

using Domain.Domain;
using Domain.DTOs.Food;

namespace DataAcess.Repos.IRepos;

public interface IRecipeRepository
{
    Task AddAsync(FullRecipeDTO recipeDTO);
    Task UpdateAsync(int id);
    Task DeleteAsync(int id);
    Task<Recipe?> GetAsync(int id);
    Task<Recipe?> GetWithoutTrackingAsync(int id);
    Task<List<Recipe>> GetAllAsync();
    Task<List<Recipe>> GetAllWithoutTrackingAsync();
    IQueryable<Recipe> GetAllQueryable();
}