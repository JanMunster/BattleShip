using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipLibrary.Models
{
    public class ShipModel
    {
        public string ShipType { get; set; }
        public int ShipSize { get; set; }
        public List<string> ShipSectionStatus { get; set; }
        public bool IsAlive { get; set; } = true;
        public List<(int, int)> Placement { get; set; } = new List<(int, int)> { (-1, -1) };
        public bool IsVertical { get; set; }

    }
}
