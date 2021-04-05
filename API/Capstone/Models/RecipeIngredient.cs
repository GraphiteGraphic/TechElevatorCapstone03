using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class RecipeIngredient
    {
        public int IngredientId { get; set; }
        public string Ingredient_name { get; set; }

        //add quantity and unit 


        public RecipeIngredient(int ingredientId, string ingredient_name)
        {
            this.IngredientId = ingredientId;
            this.Ingredient_name = ingredient_name;

        }
        public RecipeIngredient()
        {

        }
    }
}
