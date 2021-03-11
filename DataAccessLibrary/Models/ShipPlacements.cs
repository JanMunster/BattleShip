using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class ShipPlacements
    {
        public int Id { get; set; }

        public string Carrier1 { get; set; }
        public string Carrier2 { get; set; }
        public string Carrier3 { get; set; }
        public string Carrier4 { get; set; }
        public string Carrier5 { get; set; }

        public string Battleship1 { get; set; }
        public string Battleship2 { get; set; }
        public string Battleship3 { get; set; }
        public string Battleship4 { get; set; }

        public string Cruiser1 { get; set; }
        public string Cruiser2 { get; set; }
        public string Cruiser3 { get; set; }

        public string Submarine1 { get; set; }
        public string Submarine2 { get; set; }
        public string Submarine3 { get; set; }

        public string Destroyer1 { get; set; }
        public string Destroyer2 { get; set; }
    }
}
