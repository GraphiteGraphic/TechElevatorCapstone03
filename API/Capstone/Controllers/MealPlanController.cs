using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MealPlanController : AuthorizedControllerBase
    {
        //meal dao and meal plan dao

        public readonly IMealPlanDAO mealPlanDAO;
        public readonly IMealDAO mealDAO;
        public readonly IMealRecipeDAO mealRecipeDAO;

        public MealPlanController(IMealPlanDAO mealplandao, IMealDAO mealdao, IMealRecipeDAO MealRecipeDAO)
        {
            this.mealPlanDAO = mealplandao;
            this.mealDAO = mealdao;
            this.mealRecipeDAO = MealRecipeDAO;
        }

        [HttpPost]

        public ActionResult AddMealPlan(MealPlan mealPlan)
        {
            // get all meals of the logged in user from the database
            List<Meal> myMeals = mealDAO.GetMyMeals(this.UserId);

            //get all id's from those meals
            List<int> mealIds = new List<int>();
            foreach(Meal meal in myMeals)
            {
                mealIds.Add(meal.MealId);
            }

            //loop through mealplan's meal list and find meals not in the database
            List<Meal> newMeals = new List<Meal>();
            foreach (Meal meal in mealPlan.MealList)
            {
                if (!mealIds.Contains(meal.MealId))
                {
                    if (meal.recipes.Count() != 0)
                    {
                        newMeals.Add(meal);
                    }
                }
            }

            //add meals that are not in the database
            if (newMeals.Count > 0)
            {
                mealDAO.AddMeals(newMeals, this.UserId);
            }
            

            //link mealid's with their respecitve recipeid's 
            mealRecipeDAO.AddMealRecipes(newMeals);

            //post mealPlan to mealPlans to meal_plans table
            mealPlanDAO.AddMealPlan(mealPlan, this.UserId);

            //post mealPlan and meals to meal_plans_meals table
            mealPlanDAO.AddMealstoMealPlan(mealPlan);

            //return mealPlan with all new mealPlan id's (only meals with recipes in it got an id)
            return Ok(mealPlan);
            
        }

    }
}
