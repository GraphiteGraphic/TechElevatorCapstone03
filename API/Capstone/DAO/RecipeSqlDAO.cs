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
        private const string ADD_RECIPE = "Insert into recipes(user_id, recipe_name, instructions, type_id, num_servings, is_shared)VALUES(@user_id, @recipe_name, @instructions, @type_id, @num_servings, @is_shared);SELECT @@IDENTITY;";

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
                        recipe.Type = Convert.ToInt32(reader["type_id"]);
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
                        recipe.Type = Convert.ToInt32(reader["type_id"]);
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


        public Recipe AddRecipe(Recipe recipe,int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(ADD_RECIPE, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);//recipe.UserId or just this.UserId?
                    cmd.Parameters.AddWithValue("@recipe_name", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@instructions", recipe.Instructions);
                    cmd.Parameters.AddWithValue("@type_id", recipe.Type);
                    cmd.Parameters.AddWithValue("@num_servings", recipe.Servings);
                    cmd.Parameters.AddWithValue("@is_shared", recipe.IsShared);
                    recipe.RecipeId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            catch (SqlException)
            {
               
            }
            return recipe;
        }

    }
}
