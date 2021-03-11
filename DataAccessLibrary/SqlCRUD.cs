using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public class SqlCRUD  // (Create, Read, Update, Delete)

    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();

        public SqlCRUD(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ShipPlacements> GetAllData()
        {
            string sql = "select * from dbo.ShipPlacements";
            return db.LoadData<ShipPlacements, dynamic>(sql, new { }, _connectionString);
        }
    }
}
