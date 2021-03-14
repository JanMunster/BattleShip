using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipWPF
{

    public class AI
    {
        List<ShipPlacements> allPlacements;

        public AI(List<ShipPlacements> allPlacements)
        {
            this.allPlacements = allPlacements;
        }

        public List<(int, int)> GenerateAIListOfShots()
        {
            throw new NotImplementedException();
        }
    }
}
