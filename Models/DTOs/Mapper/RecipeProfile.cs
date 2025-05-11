using AutoMapper;
using Domain.Domain;
using Domain.DTOs.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Mapper
{
    public class RecipeProfile:Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeWithNutritionDTO>()
           .ForMember(dest => dest.Calories_100g, opt => opt.MapFrom(src => src.Nutrition.Calories_100g))
           .ForMember(dest => dest.Fat_100g, opt => opt.MapFrom(src => src.Nutrition.Fat_100g))
           .ForMember(dest => dest.Sugar_100g, opt => opt.MapFrom(src => src.Nutrition.Sugar_100g))
           .ForMember(dest => dest.Protein_100g, opt => opt.MapFrom(src => src.Nutrition.Protein_100g))
           .ForMember(dest => dest.Carb_100, opt => opt.MapFrom(src => src.Nutrition.Carb_100))
           .ForMember(dest => dest.Type,opt => opt.MapFrom(src => src.Nutrition.Type))
           .ForMember(dest => dest.IngredientNames, opt => opt.MapFrom(src =>
          src.Recipe_Ingredient.Select(ri => ri.Ingredient.Ingredient_Name).ToList()
      )).ReverseMap();
        }
    }
}
