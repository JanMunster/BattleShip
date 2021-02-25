using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipLibrary.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public List<ShipModel> Ships { get; set; } = new List<ShipModel>();
        public bool[,] ShotFired { get; set; } = new bool[10, 10];
        public bool[,] ShipSectionHere { get; set; } = new bool[10, 10];
        public bool IsHumanStarting { get; set; } = true;
        public bool IsAIon { get; set; }
    }
}
