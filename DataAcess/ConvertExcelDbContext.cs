using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using DataAcess.Configuration;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs.Food;

namespace DataAcess.ExternalDb;

public partial class ConvertExcelDbContext : DbContext
{

    public ConvertExcelDbContext(DbContextOptions<ConvertExcelDbContext> options): base(options)
    {

    }
    public DbSet<Recipe> Recipe { get; set; }
    public DbSet<Nutrition> Nutrition { get; set; }
    public DbSet<Ingredient> Ingredient { get; set; }
    public DbSet<Recipe_Ingredient> Recipe_Ingredient { get; set; }
    public DbSet<RecipeRawDTO> RecipeRawDTO { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Apply separate configuration classes
        builder.ApplyConfiguration(new RecipeConfiguration());
        builder.ApplyConfiguration(new NutritionConfiguration());
        builder.ApplyConfiguration(new IngredientConfiguration());
        builder.ApplyConfiguration(new RecipeIngredientConfiguration());
        builder.Entity<RecipeRawDTO>().HasNoKey();
    }
}
