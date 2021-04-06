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
        private readonly IRecipeDAO recipeDAO;
        

        public RecipesController(IRecipeDAO recipedao)
        {
            this.recipeDAO = recipedao;
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
            recipe = recipeDAO.AddRecipe(recipe);
            return Ok(recipe);
        }

   }
}
