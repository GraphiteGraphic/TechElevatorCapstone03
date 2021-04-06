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
    public class RecipeIngredientsController : AuthorizedControllerBase
    {
        private readonly IRecipeIngredientDAO recipeIngredientsDAO;

        public RecipeIngredientsController(IRecipeIngredientDAO recipeIngredientsDAO)
        {
            this.recipeIngredientsDAO = recipeIngredientsDAO;
        }

        [HttpGet("{recipeId}")]
        public ActionResult GetRecipe(int recipeId)
        {
            // call recipeDAO to get list of recipes, get recipe, then check parameters

            List<RecipeIngredient> listOfRecipeIngredients = recipeIngredientsDAO.GetMyRecipeIngredients(recipeId, this.UserId);
            return Ok(listOfRecipeIngredients);

            //if (recipe.IsShared)
            //{
            //    List<RecipeIngredient> listOfRecipeIngredients = recipeIngredientsDAO.GetPublicRecipeIngredients(recipe.RecipeId);
            //    return Ok(listOfRecipeIngredients);
            //}
            //else if (recipe.UserId == this.UserId)
            //{
            //    List<RecipeIngredient> listOfRecipeIngredients = recipeIngredientsDAO.GetMyRecipeIngredients(recipe.RecipeId, this.UserId);
            //    return Ok(listOfRecipeIngredients);
            //}
            //return Forbid();
        }
    }
}
