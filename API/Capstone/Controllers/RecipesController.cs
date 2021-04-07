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
        

        public RecipesController(IRecipeDAO recipedao, IIngredientDAO IngredientDAO)
        {
            this.recipeDAO = recipedao;
            this.ingredientDAO = IngredientDAO;
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
            if (recipe.NewIngredients.Count() > 0)
            {
                ingredientDAO.AddIngredients(recipe.NewIngredients);
            }
            

            recipe = recipeDAO.AddRecipe(recipe, this.UserId);
            return Created($"/recipes/{recipe.RecipeId}",recipe);
        }

   }
}
