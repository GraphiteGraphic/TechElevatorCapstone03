using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IRecipeIngredientDAO
    {
        List<RecipeIngredient> GetMyRecipeIngredients(int recipeId, int userId);
        List<RecipeIngredient> GetPublicRecipeIngredients(int recipeId);
    }
}