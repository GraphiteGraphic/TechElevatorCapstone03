using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public int TimeOfDay { get; set; }
        public int UserId { get; set; }

        public Meal(int mealId, string mealName, int timeofday)
        {
            this.MealId = mealId;
            this.MealName = mealName;
            this.TimeOfDay = timeofday;
        }

        public Meal()
        {

        }

    }
}
