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

        public MealPlanController(IMealPlanDAO mealplandao, IMealDAO mealdao)
        {
            this.mealPlanDAO = mealplandao;
            this.mealDAO = mealdao;
        }

        [HttpPost]

        public ActionResult AddMealPlan(List<Meal> mealPlan)
        {
            List<Meal> myMeals = mealDAO.GetMyMeals(this.UserId);
            List<int> mealIds = new List<int>();
            foreach(Meal meal in myMeals)
            {
                mealIds.Add(meal.MealId);
            }
            List<Meal> newMeals = new List<Meal>();
            foreach (Meal meal in mealPlan)
            {
                if(mealIds.Contains(meal.MealId))
                {
                    newMeals.Add(meal);
                }
            }
            if (newMeals.Count > 0)
            {
                mealDAO.AddMeals(newMeals, this.UserId);
            }

            return Ok();
            
        }

    }
}
