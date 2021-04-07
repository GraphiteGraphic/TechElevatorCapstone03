using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class IngredientSqlDAO : IIngredientDAO
    {
        private const string GET_ALL_INGREDIENTS = "SELECT * FROM ingredients";
        private readonly string connectionString;
        public IngredientSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString; 
        }

        public List<Ingredient> GetAllIngredients()
        {
            List<Ingredient> allIngredients = new List<Ingredient>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(GET_ALL_INGREDIENTS, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Ingredient ingredient = new Ingredient();
                        ingredient.IngredientId = Convert.ToInt32(rdr["ingredient_id"]);
                        ingredient.IngredientName = Convert.ToString(rdr["ingredient_name"]);

                        allIngredients.Add(ingredient);
                    }
                    return allIngredients;
                }

            }
            catch (SqlException)
            {
                throw;
            }
        }
    }
}

            

        
            
    

