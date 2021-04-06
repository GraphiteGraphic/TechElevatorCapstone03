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

        [HttpGet("publicrecipes/{recipeId}")]
        public ActionResult GetPublicRecipeIngredients(int recipeId)
        {
            List<RecipeIngredient> listOfRecipeIngredients = recipeIngredientsDAO.GetPublicRecipeIngredients(recipeId);
            return Ok(listOfRecipeIngredients);
        }

        [HttpGet("myrecipes/{recipeId}")]
        [Authorize]
        public ActionResult GetMyRecipeIngredients(int recipeId)
        {
            List<RecipeIngredient> listOfRecipeIngredients = recipeIngredientsDAO.GetMyRecipeIngredients(recipeId, this.UserId);
            return Ok(listOfRecipeIngredients);
        }



    }
}
