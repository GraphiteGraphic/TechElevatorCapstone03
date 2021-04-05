using System.Collections.Generic;

namespace Capstone.Models
{
    public class Recipe
    {
        public int RecipeId { get; set;  }

        public string RecipeName { get; set; }

        public int UserId { get; set; } // user id, Debated on having an int for userID or an instance of User for our userId.  
        public string Instructions { get; set; }
        public Type Type { get; set; }

        public int Servings { get; set; }

        public bool IsShared { get; set; }

       


        public Recipe(int recipeId, string recipeName, int userId, string instructions, Type type, int servings, bool isshared)
        {
            this.RecipeId = recipeId;
            this.RecipeName = recipeName;
            this.UserId = userId;
            this.Instructions = instructions;
            this.Type = type;
            this.Servings = servings;
            this.IsShared = isshared;
           
        }
        public Recipe()
        {

        }

    }






}
