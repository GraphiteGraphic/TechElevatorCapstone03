using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RecipesController : AuthorizedControllerBase  //ControllerBase
    {
        public readonly IRecipeDAO recipeDAO;
        public readonly IIngredientDAO ingredientDAO;
        public readonly IRecipeIngredientDAO recipeIngredientDAO;
        

        public RecipesController(IRecipeDAO recipedao, IIngredientDAO IngredientDAO, IRecipeIngredientDAO RecipeIngredientDAO)
        {
            this.recipeDAO = recipedao;
            this.ingredientDAO = IngredientDAO;
            this.recipeIngredientDAO = RecipeIngredientDAO;
        }

        [HttpGet]
        public ActionResult GetPublicRecipes()
        {
            List<Recipe> listOfRecipe = recipeDAO.GetPublicRecipes();
            return Ok(listOfRecipe);
        }

        
       [HttpGet("myrecipes")]
       [Authorize]
        public ActionResult GetPrivateRecipes()
        {
            List<Recipe> privateListOfRecipe = recipeDAO.GetPrivateRecipes(this.UserId);
            return Ok(privateListOfRecipe);
        }


        [HttpPost]
        public ActionResult AddRecipe(Recipe recipe)
        {
            // add new ingredients into the ingredient table
            if (recipe.NewIngredients.Count() > 0)
            {
                ingredientDAO.AddIngredients(recipe.NewIngredients);
            }

            // add recipe into recipes table
            recipe = recipeDAO.AddRecipe(recipe, this.UserId);

            // add recipe_id, ingredient_id, ingredient_qty, and ingredient_unit into recipes_ingredient table
            recipeIngredientDAO.AddIngredients(recipe.IngredientList, recipe.RecipeId);

            // return recipe so front end can use the recipe id
            // update recipe object so it has the ingredient id's in them?  front end doesn't need as of 4/7
            return Created($"/recipes/{recipe.RecipeId}",recipe);
        }

   }
}
