using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class MealPlanSqlDAO : IMealPlanDAO
    {
        private readonly string connectionString;

        public MealPlanSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
    }
}
