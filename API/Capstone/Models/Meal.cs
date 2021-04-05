using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Meal
    {
        public int MealId { get; }
        public string MealName { get; set; }
        public TimeOfDay TimeOfDay { get; set; }

        public Meal(int mealId, string mealName, TimeOfDay timeofday)
        {
            this.MealId = mealId;
            this.MealName = mealName;
            this.TimeOfDay = timeofday;
        }

    }
}
