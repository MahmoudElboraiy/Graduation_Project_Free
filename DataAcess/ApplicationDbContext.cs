using DataAcess.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Domain;
using Domain.Domain;
using Domain.DTOs.Food;
namespace DataAcess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // this is pull
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Nutrition> Nutrition { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe_Ingredient> Recipe_Ingredient { get; set; }
        public DbSet<RecipeRawDTO> RecipeRawDTO { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Apply separate configuration classes
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RecipeConfiguration());
            builder.ApplyConfiguration(new NutritionConfiguration());
            builder.ApplyConfiguration(new IngredientConfiguration());
            builder.ApplyConfiguration(new RecipeIngredientConfiguration());
            builder.Entity<RecipeRawDTO>().HasNoKey();
        }

    }
}
