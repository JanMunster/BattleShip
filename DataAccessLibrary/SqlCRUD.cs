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

        public void CreateShipPlacement(ShipPlacements shipPlacements)
        {
            string sql = "insert into dbo.ShipPlacements (Carrier1,Carrier2,Carrier3,Carrier4,Carrier5," +
                "Battleship1,Battleship2,Battleship3,Battleship4,Cruiser1,Cruiser2,Cruiser3,Submarine1," +
                "Submarine2,Submarine3,Destroyer1,Destroyer2) values (@Carrier1, @Carrier2, @Carrier3," +
                "@Carrier4, @Carrier5, @Battleship1, @Battleship2, @Battleship3, @Battleship4, @Cruiser1," +
                "@Cruiser2, @Cruiser3, @Submarine1, @Submarine2, @Submarine3, @Destroyer1, @Destroyer2);";

            db.SaveData(sql,
                new
                {
                    shipPlacements.Carrier1,
                    shipPlacements.Carrier2,
                    shipPlacements.Carrier3,
                    shipPlacements.Carrier4,
                    shipPlacements.Carrier5,
                    shipPlacements.Battleship1,
                    shipPlacements.Battleship2,
                    shipPlacements.Battleship3,
                    shipPlacements.Battleship4,
                    shipPlacements.Cruiser1,
                    shipPlacements.Cruiser2,
                    shipPlacements.Cruiser3,
                    shipPlacements.Submarine1,
                    shipPlacements.Submarine2,
                    shipPlacements.Submarine3,
                    shipPlacements.Destroyer1,
                    shipPlacements.Destroyer2
                },
                    _connectionString);
        }
    }
}
