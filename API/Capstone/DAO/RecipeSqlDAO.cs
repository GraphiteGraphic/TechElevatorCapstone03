using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Type = Capstone.Models.Type;

namespace Capstone.DAO
{
    public class RecipeSqlDAO : IRecipeDAO
    {
        private const string GET_RECIPES = @"select * from recipes where is_shared = 1";
        private const string GET_MY_RECIPES = "select * from recipes where user_id = @userID";
        private readonly string connectionString;

        public RecipeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Recipe> GetPublicRecipes()
        {
            //Go to our database
            List<Recipe> listOfRecipes = new List<Recipe>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(GET_RECIPES, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.RecipeId = Convert.ToInt32(reader["recipe_id"]);
                        recipe.RecipeName = Convert.ToString(reader["recipe_name"]);
                        recipe.UserId = Convert.ToInt32(reader["user_id"]);
                        recipe.Instructions = Convert.ToString(reader["instructions"]);
                        recipe.Type = (Type)Convert.ToInt32(reader["type_id"]);
                        recipe.Servings = Convert.ToInt32(reader["num_servings"]);
                        recipe.IsShared = Convert.ToBoolean(reader["is_shared"]);

                        listOfRecipes.Add(recipe);
                    }
                    return listOfRecipes;
                }
            }
            catch (SqlException)
            {
                throw;
            }

        }

        public List<Recipe> GetPrivateRecipes(int userId)
        {
            List<Recipe> PrivateListOfRecipes = new List<Recipe>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(GET_MY_RECIPES, conn);
                    cmd.Parameters.AddWithValue("@userID", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.RecipeId = Convert.ToInt32(reader["recipe_id"]);
                        recipe.RecipeName = Convert.ToString(reader["recipe_name"]);
                        recipe.UserId = Convert.ToInt32(reader["user_id"]);
                        recipe.Instructions = Convert.ToString(reader["instructions"]);
                        recipe.Type = (Type)Convert.ToInt32(reader["type_id"]);
                        recipe.Servings = Convert.ToInt32(reader["num_servings"]);
                        recipe.IsShared = Convert.ToBoolean(reader["is_shared"]);

                        PrivateListOfRecipes.Add(recipe);
                    }
                    return PrivateListOfRecipes;
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }


    }
}
