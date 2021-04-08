using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class MealPlanSqlDAO : IMealPlanDAO
    {
        private const string ADD_MEALPLAN = "INSERT INTO meal_plans (name, user_id) VALUES (@name,@userid); SELECT @@IDENTITY";
        private const string ADD_MEALS_TO_MEALPLAN = "INSERT INTO meal_plans_meals (meal_plan_id, meal_id) VALUES (@mealPlanId, @mealId)";
        private readonly string connectionString;

        public MealPlanSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public MealPlan AddMealPlan(MealPlan newMealPlan, int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(ADD_MEALPLAN, conn);
                    cmd.Parameters.AddWithValue("@name", newMealPlan.Name);
                    cmd.Parameters.AddWithValue("@userid", userId);
                    newMealPlan.MealPlanId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {

            }
            return newMealPlan;
        }

        public MealPlan AddMealstoMealPlan(MealPlan newMealPlan)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    foreach (Meal meal in newMealPlan.MealList)
                    {
                        if (meal.MealId != 0)
                        {
                            SqlCommand cmd = new SqlCommand(ADD_MEALS_TO_MEALPLAN, conn);
                            cmd.Parameters.AddWithValue("@mealPlanId", newMealPlan.MealPlanId);
                            cmd.Parameters.AddWithValue("@mealId", meal.MealId);
                            cmd.ExecuteScalar();
                        }
                    }
                }
            }
            catch
            {

            }
            return newMealPlan;
        }
    }
}
