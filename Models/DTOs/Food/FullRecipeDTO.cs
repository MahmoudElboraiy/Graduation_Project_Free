using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Food
{
    public class FullRecipeDTO
    {
        public string Name { get; set; }

        public int Time { get; set; }

        public string Type { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Preparation_Method { get; set; }

        public List<IngredientDto> ingredientDtos { get; set; } = new List<IngredientDto>();

        public double Calories_100g { get; set; }

        public double Fat_100g { get; set; }

        public double Sugar_100g { get; set; }

        public double Protein_100g { get; set; }

        public double Carb_100 { get; set; }


    }
}
