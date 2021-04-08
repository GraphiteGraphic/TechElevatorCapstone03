using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class MealPlan
    {
        public int MealPlanId { get; }

        public string Name { get; set; }
        public int UserId { get; set; }

        public MealPlan(int mealplanid, string name, int userId)
        {
            this.MealPlanId = mealplanid;
            this.Name = name;
            this.UserId = userId;
        }


        // maybe need a time_priod property, can update later if need be.
    }
}
