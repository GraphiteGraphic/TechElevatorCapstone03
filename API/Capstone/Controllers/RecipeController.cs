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
    public class RecipeController : AuthorizedControllerBase  //ControllerBase
    {
        private readonly IRecipeDAO recipeDAO;
        int userId;

        public RecipeController(IRecipeDAO recipedao)
        {
            this.recipeDAO = recipedao;
        }

        [HttpGet]

        public ActionResult GetPublicRecipes()
        {
            List<Recipe> listOfRecipe = recipeDAO.GetPublicRecipes();
            return Ok(listOfRecipe);
        }

        [Authorize]
        [HttpGet("{userId}")]

        public ActionResult GetPrivateRecipes(int userId)
        {
            List<Recipe> privateListOfRecipe = recipeDAO.GetPrivateRecipes(userId);
            return Ok(privateListOfRecipe);
        }
    }
}
