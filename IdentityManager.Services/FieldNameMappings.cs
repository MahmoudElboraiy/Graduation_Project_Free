using Domain.Domain;
using Domain.DTOs.Food;

public static class FieldNameMappings
{
    public static readonly Dictionary<Type, Dictionary<string, Dictionary<string, string>>> Mappings =
        new()
        {
            {
                typeof(Recipe),
                new Dictionary<string, Dictionary<string, string>>
                {
                    { "ar", new Dictionary<string, string>
                        {
                            { "Recipe_Id", "معرف الوصفة" },
                            { "Recipe_Name", "اسم الوصفة" },
                            { "Time", "الوقت" },
                            { "Description", "الوصف" },
                            { "Preparation_Method", "طريقة التحضير" },
                            { "Nutrition", "القيمة الغذائية" }
                        }
                    },
                    { "en", new Dictionary<string, string>
                        {
                            { "Recipe_Id", "Recipe_Id" },
                            { "Recipe_Name", "Recipe_Name" },
                            { "Time", "Time" },
                            { "Description", "Description" },
                            { "Preparation_Method", "Preparation_Method" },
                            { "Nutrition", "Nutrition" }
                        }
                    }
                }
            },
            {
                typeof(Ingredient),
                new Dictionary<string, Dictionary<string, string>>
                {
                    { "ar", new Dictionary<string, string>
                        {
                            { "Ingredient_Id", "معرف المكون" },
                            { "Ingredient_Name", "اسم المكون" },
                            { "Recipe_Ingredient", "وصفات المكون" },
                            { "Type", "النوع" }
                        }
                    },
                    { "en", new Dictionary<string, string>
                        {
                            { "Ingredient_Id", "Ingredient_Id" },
                            { "Ingredient_Name", "Ingredient_Name" },
                            { "Recipe_Ingredient", "Recipe_Ingredient" },
                            { "Type", "Type" }
                        }
                    }
                }
            },{
                typeof(Nutrition),
                new Dictionary<string, Dictionary<string, string>>
                {
                    { "ar", new Dictionary<string, string>
                        {
                            { "Recipe_Id", "معرف الوصفة" },
                            { "Calories_100g", "السعرات الحرارية لكل 100جم" },
                            { "Fat_100g", "الدهون لكل 100جم" },
                            { "Sugar_100g", "السكر لكل 100جم" },
                            { "Protein_100g", "البروتين لكل 100جم" },
                            { "Carb_100", "الكربوهيدرات لكل 100جم" },
                            { "Type", "النوع" }
                        }
                    },
                    { "en", new Dictionary<string, string>
                        {
                            { "Recipe_Id", "Recipe_Id" },
                            { "Calories_100g", "Calories_100g" },
                            { "Fat_100g", "Fat_100g" },
                            { "Sugar_100g", "Sugar_100g" },
                            { "Protein_100g", "Protein_100g" },
                            { "Carb_100", "Carb_100" },
                            { "Type", "Type" }
                        }
                    }
                }
            },{
                typeof(Recipe_Ingredient),
                new Dictionary<string, Dictionary<string, string>>
                {
                    { "ar", new Dictionary<string, string>
                        {
                            { "RecipeId", "معرف الوصفة" },
                            { "Ingredient_Id", "معرف المكون" },
                            { "Amount", "الكمية" }
                        }
                    },
                    { "en", new Dictionary<string, string>
                        {
                            { "RecipeId", "RecipeId" },
                            { "Ingredient_Id", "Ingredient_Id" },
                            { "Amount", "Amount" }
                        }
                    }
                }
            },
                        {
                typeof(RecipeWithNutritionDTO),
                new Dictionary<string, Dictionary<string, string>>
                {
                    { "ar", new Dictionary<string, string>
                        {
                            { "Recipe_Name", "اسم الوصفة" },
                            { "Description", "الوصف" },
                            { "Preparation_Method", "طريقة التحضير" },
                            { "Time", "الوقت" },
                            { "Calories_100g", "السعرات الحرارية لكل 100جم" },
                            { "Fat_100g", "الدهون لكل 100جم" },
                            { "Sugar_100g", "السكر لكل 100جم" },
                            { "Protein_100g", "البروتين لكل 100جم" },
                            { "Carb_100", "الكربوهيدرات لكل 100جم" },
                            { "IngredientNames", "اسماء المكونات" },
                            { "Type", "النوع" }
                        }
                    },
                    { "en", new Dictionary<string, string>
                        {
                            { "Recipe_Name", "Recipe_Name" },
                            { "Description", "Description" },
                            { "Preparation_Method", "Preparation_Method" },
                            { "Time", "Time" },
                            { "Calories_100g", "Calories_100g" },
                            { "Fat_100g", "Fat_100g" },
                            { "Sugar_100g", "Sugar_100g" },
                            { "Protein_100g", "Protein_100g" },
                            { "Carb_100", "Carb_100" },
                            { "IngredientNames", "IngredientNames" },
                            { "Type", "Type" }
                        }
                    }
                }
            }

    // تقدر تضيف هنا موديلات تانية بنفس الشكل
        };
}
