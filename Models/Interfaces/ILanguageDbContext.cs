using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILanguageDbContext
    {
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Nutrition> Nutrition { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe_Ingredient> Recipe_Ingredient { get; set; }
    }
}
