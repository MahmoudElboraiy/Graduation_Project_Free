using DataAcess.Configuration;
using Domain.Domain;
using Domain.DTOs.Food;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class Arabic_ApplicationDbContext : DbContext, ILanguageDbContext
    {
        public Arabic_ApplicationDbContext(DbContextOptions<Arabic_ApplicationDbContext> options) : base(options)
        {
            // this is pull
        }
        public DbSet<Image> Image { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Nutrition> Nutrition { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe_Ingredient> Recipe_Ingredient { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Nutrition>(entity =>
            {
                entity.ToTable("التغذية", schema: "dbo");
                entity.Property(e => e.Recipe_Id).HasColumnName("بطاقة تعريف");
                entity.Property(e => e.Calories_100g).HasColumnName("السعرات الحرارية");
                entity.Property(e => e.Fat_100g).HasColumnName("سمين");
                entity.Property(e => e.Sugar_100g).HasColumnName("سكر");
                entity.Property(e => e.Protein_100g).HasColumnName("بروتين");
                entity.Property(e => e.Carb_100).HasColumnName("الكربوهيدرات");
                entity.Property(e => e.Type).HasColumnName("النوع");
            });
            builder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("المكونات", schema: "dbo");
                entity.Property(e => e.Ingredient_Id).HasColumnName("بطاقة تعريف");
                entity.Property(e => e.Ingredient_Name).HasColumnName("المكونات");
                entity.Property(e => e.Type).HasColumnName("النوع");

            });
            builder.Entity<Recipe>(entity =>
            {
                entity.ToTable("الوصفات", schema: "dbo");
                entity.Property(e => e.Recipe_Id).HasColumnName("بطاقة تعريف");
                entity.Property(e => e.Recipe_Name).HasColumnName("اسم");
                entity.Property(e => e.Time).HasColumnName("دقائق");
                entity.Property(e => e.Preparation_Method).HasColumnName("طريقة التحضير");
                entity.Property(e => e.Description).HasColumnName("وصف");
            });
            builder.Entity<Recipe_Ingredient>(entity =>
            {
                entity.ToTable("وصفة_المكونات", schema: "dbo");
                entity.Property(e => e.RecipeId).HasColumnName("بطاقة تعريف الوصفة");
                entity.Property(e => e.Ingredient_Id).HasColumnName("بطاقة تعريف المكون");
                entity.Property(e => e.Amount).HasColumnName("كمية");
            });
            builder.ApplyConfiguration(new RecipeConfiguration());
            builder.ApplyConfiguration(new NutritionConfiguration());
            builder.ApplyConfiguration(new IngredientConfiguration());
            builder.ApplyConfiguration(new RecipeIngredientConfiguration());
        }
    }
}
