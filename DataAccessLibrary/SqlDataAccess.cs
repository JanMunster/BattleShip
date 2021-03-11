using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataAccessLibrary
{
    public class SqlDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            // The using statement connects to the database. It also ensures correct closing of connection.
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // 'Query' is a dapper command for querying database.
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }

            
        }
        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }
    }
}
