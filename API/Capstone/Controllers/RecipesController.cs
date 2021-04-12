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
        [Authorize]
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

        [HttpPut]
        [Authorize]
        public ActionResult ModifyRecipe(Recipe editRecipe)
        {
            // get my original recipe with no updates
            List<Recipe> myRecipes = recipeDAO.GetPrivateRecipes(this.UserId);
            //GetMyRecipeIngredients
            Recipe originalRecipe = new Recipe();
            foreach (Recipe recipe in myRecipes)
            {
                if (recipe.RecipeId == editRecipe.RecipeId)
                {
                    originalRecipe = recipe; // meal plan with no edits
                }
            }
            originalRecipe.IngredientList = recipeIngredientDAO.GetMyRecipeIngredients(originalRecipe.RecipeId, this.UserId);

            List<int> originalIngredientIds = new List<int>();
            foreach(RecipeIngredient ingredient in originalRecipe.IngredientList)
            {
                originalIngredientIds.Add(ingredient.IngredientId);
            }

            //update recipe details
            recipeDAO.UpdateRecipe(editRecipe);

            //add new ingredients into ingredients table
            if (editRecipe.NewIngredients.Count() > 0)
            {
                ingredientDAO.AddIngredients(editRecipe.NewIngredients);
            }

            //orignal recipe: pbnj has ingredient list [5,6,7].   [5 2oz, 6 2oz, 7 2slices]
            // edit recipe:                                     [5 1oz, 6 2oz, 7 2slices, 8 2oz] [6 2oz, 7 2slices]

            //loop through new recipe ingredient list
            //i check if the old ingredients are in the new list by id's
            //if the old ingredient is in the new list, check to see what needs to be updated on the properties
            //if the old ingredient is not in the new list, we need to delete that item from the recipes_ingredients
            foreach (RecipeIngredient eIngredient in editRecipe.IngredientList)
            {
                //insert the new ingredients into recipes_ingredients
                if (editRecipe.NewIngredients.Contains(eIngredient.IngredientName))
                {
                    recipeIngredientDAO.AddIngredient(eIngredient, editRecipe.RecipeId);
                }
                else
                {
                    foreach (RecipeIngredient oIngredient in originalRecipe.IngredientList)
                    {
                        if (eIngredient.IngredientId == oIngredient.IngredientId)
                        {
                            //check qty, check unit.  update if necessary
                        }
                        // id's didn't match. old was deleted
                        else
                        {
                            //delete the old ingredient from the recipes_ingredients list
                        }
                    }
                }
            }

            return Ok(editRecipe);
        }

   }
}
