using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public interface IMealPlanDAO
    {
        MealPlan AddMealPlan(MealPlan newMealPlan, int userId);
        MealPlan AddMealstoMealPlan(MealPlan newMealPlan);
    }
}
