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
        private const string ADD_MEALPLAN = "INSERT INTO meal_plans (name, user_id, meal_indices) VALUES (@name,@userid,@indices); SELECT @@IDENTITY";
        private const string ADD_MEALS_TO_MEALPLAN = "INSERT INTO meal_plans_meals (meal_plan_id, meal_id) VALUES (@mealPlanId, @mealId)";
        private const string GET_MY_MEALPLANS = "Select * from meal_plans where user_id=@userid";
        private const string SELECT_MEALS_IN_MEALPLAN = "select meal_id from meal_plans_meals where meal_plan_id = @mealplanId";
        private const string SELECT_RECIPES_IN_MEAL = "select recipe_id from meals_recipes where meal_id = @mealId";
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
                    cmd.Parameters.AddWithValue("@indices", newMealPlan.indices);
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

        public List<MealPlan> GetMyMealPlans(int userId)
        {
            List<MealPlan> myMealPlans = new List<MealPlan>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(GET_MY_MEALPLANS, conn);
                    cmd.Parameters.AddWithValue("@userid", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MealPlan mealPlan = new MealPlan();
                        mealPlan.MealPlanId = Convert.ToInt32(reader["meal_plan_id"]);
                        mealPlan.Name = Convert.ToString(reader["name"]);
                        mealPlan.UserId = userId;
                        mealPlan.indices = Convert.ToString(reader["meal_indices"]);
                        myMealPlans.Add(mealPlan);

                    }
                }
            }
            catch
            {

            }
            return myMealPlans;
        }
        
        public List<MealPlan> GetMyMealList(List<MealPlan> myMealPlans, int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    //LOOP THROUGH ALL MEALPLANS THIS USER HAS
                    foreach(MealPlan mealPlan in myMealPlans)
                    {
                        //initiate the meal list to go into this mealplan
                        mealPlan.MealList = new List<Meal>();

                        //get all the meal_ids of this specific mealplan
                        SqlCommand cmd = new SqlCommand(SELECT_MEALS_IN_MEALPLAN, conn);
                        cmd.Parameters.AddWithValue("@mealplanId", mealPlan.MealPlanId);
                        SqlDataReader rdr = cmd.ExecuteReader();
                        
                        //indices is a string that holds a 0 where an empty meal is, or a 1 where an actual meal is
                        //loop through the indices and add a meal with its properties depending on if there is 
                        //information in that meal or if it is an empty meal
                        for (int j = 0; j < mealPlan.indices.Length; j++)
                        {
                            if (mealPlan.indices[j] == '1')
                            {
                                if (rdr.Read())
                                {
                                    Meal meal = new Meal();
                                    meal.MealId = Convert.ToInt32(rdr["meal_id"]);
                                    mealPlan.MealList.Add(meal);
                                }
                            }
                            else
                            {
                                Meal meal = new Meal();
                                mealPlan.MealList.Add(meal);
                            }
                        }
                        rdr.Close();

                        //LOOP THROUGH ALL MEALS THIS MEALPLAN HAS
                        foreach(Meal meal in mealPlan.MealList)
                        {
                            //initiate the recipe list to go into this meal
                            meal.recipes = new List<Recipe>();

                            //get all the recipeIds of this specific meal
                            SqlCommand cmd2 = new SqlCommand(SELECT_RECIPES_IN_MEAL, conn);
                            cmd2.Parameters.AddWithValue("@mealId", meal.MealId);
                            SqlDataReader rdr2 = cmd2.ExecuteReader();
                            while (rdr2.Read())
                            {
                                Recipe recipe = new Recipe();
                                recipe.RecipeId = Convert.ToInt32(rdr2["recipe_id"]);
                                meal.recipes.Add(recipe);
                            }
                            rdr2.Close();

                            //get the rest of the recipe info (besides the ingredient list)
                            foreach (Recipe recipe in meal.recipes)
                            {
                                SqlCommand cmd3 = new SqlCommand("select * from recipes where recipe_id = @recipeId", conn);
                                cmd3.Parameters.AddWithValue("@recipeId", recipe.RecipeId);
                                SqlDataReader rdr3 = cmd3.ExecuteReader();
                                while (rdr3.Read())
                                {
                                    recipe.RecipeName = Convert.ToString(rdr3["recipe_name"]);
                                    recipe.UserId = Convert.ToInt32(rdr3["user_id"]);
                                    recipe.Instructions = Convert.ToString(rdr3["instructions"]);
                                    recipe.Type = Convert.ToInt32(rdr3["type_id"]);
                                    recipe.Servings = Convert.ToInt32(rdr3["num_servings"]);
                                    recipe.IsShared = Convert.ToBoolean(rdr3["is_shared"]);
                                }
                                rdr3.Close();

                                //get the ingredient list for each recipe
                                recipe.IngredientList = new List<RecipeIngredient>();
                                cmd3 = new SqlCommand("select * from recipes_ingredients where recipe_id = @recipeId", conn);
                                cmd3.Parameters.AddWithValue("@recipeId", recipe.RecipeId);
                                rdr3 = cmd3.ExecuteReader();
                                while (rdr3.Read())
                                {
                                    RecipeIngredient r = new RecipeIngredient();
                                    r.IngredientId = Convert.ToInt32(rdr3["ingredient_id"]);
                                    r.Quantity = Convert.ToDecimal(rdr3["ingredient_qty"]);
                                    r.Unit = Convert.ToString(rdr3["ingredient_unit"]);
                                    r.RecipeId = Convert.ToInt32(rdr3["recipe_id"]);
                                    recipe.IngredientList.Add(r);
                                }
                                rdr3.Close();

                                //get the ingredient name for each recipe ingredient
                                foreach (RecipeIngredient ingredient in recipe.IngredientList)
                                {
                                    cmd3 = new SqlCommand("select ingredient_name from ingredients where ingredient_id=@ingredientId", conn);
                                    cmd3.Parameters.AddWithValue("@ingredientId", ingredient.IngredientId);
                                    rdr3 = cmd3.ExecuteReader();
                                    while (rdr3.Read())
                                    {
                                        ingredient.IngredientName = Convert.ToString(rdr3["ingredient_name"]);
                                    }
                                    rdr3.Close();
                                }
                            }
                        }

                    }
                
                }
            }
            catch
            {

            }
            return myMealPlans;
        }
    }
}
