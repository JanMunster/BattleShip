using BattleShipLibrary.ExtensionMethods;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BattleshipWPF
{

    public class AI
    {
        List<ShipPlacements> allPlacements;
        public int[,] CountPlacements { get; set; } = new int[10,10];
        public List<(int, int)> RandomShots { get; set; } = new List<(int, int)>();

        public AI(List<ShipPlacements> allPlacements, List<(int, int)> listOfShots)
        {
            this.allPlacements = allPlacements;
            this.RandomShots = listOfShots;
        }

        public List<(int, int)> GenerateAIListOfShots()
        {
            List<(int, int)> OptimizedShotList = new List<(int, int)>();            
            List<(int, int)> Priorities = new List<(int, int)>();

            foreach (ShipPlacements placements in allPlacements)
            {
               ExtractTuplesFromOneShip(placements.Carrier1);
               ExtractTuplesFromOneShip(placements.Carrier2);
               ExtractTuplesFromOneShip(placements.Carrier3);
               ExtractTuplesFromOneShip(placements.Carrier4);
               ExtractTuplesFromOneShip(placements.Carrier5);

               ExtractTuplesFromOneShip(placements.Battleship1);
               ExtractTuplesFromOneShip(placements.Battleship2);
               ExtractTuplesFromOneShip(placements.Battleship3);
               ExtractTuplesFromOneShip(placements.Battleship4);

               ExtractTuplesFromOneShip(placements.Cruiser1);
               ExtractTuplesFromOneShip(placements.Cruiser2);
               ExtractTuplesFromOneShip(placements.Cruiser3);

               ExtractTuplesFromOneShip(placements.Submarine1);
               ExtractTuplesFromOneShip(placements.Submarine2);
               ExtractTuplesFromOneShip(placements.Submarine3);

               ExtractTuplesFromOneShip(placements.Destroyer1);
               ExtractTuplesFromOneShip(placements.Destroyer2);

                
            }
            //Trace.WriteLine("")
            CountPlacements.PrintCountPlacements();

            while (ContainsNonZeros(CountPlacements))
            {
                int maxValue = CountPlacements.Cast<int>().Max();

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if(maxValue == CountPlacements[i, j])
                        {
                            Priorities.Insert(0, (i, j));
                            CountPlacements[i, j] = 0;
                        }
                    }
                }
            }

            foreach ((int,int) betterShot in Priorities)
            {
                RandomShots.Remove(betterShot);
                RandomShots.Add(betterShot);
            }

            Trace.WriteLine("Improved random shots:");
            RandomShots.PrintListOfShots();

            return RandomShots;
        }

        private bool ContainsNonZeros(int[,] countPlacements)
        {
            bool foundNonZero = false;
            foreach (int number in countPlacements)
            {
                if (number != 0)
                {
                    foundNonZero = true;
                    break;
                }
            }
            return foundNonZero;
        }

        private void ExtractTuplesFromOneShip(String ship)
        {
            (int, int) tempTuple = (0, 0);
            tempTuple.Item1 = Convert.ToInt32(ship.Substring(0,1));
            tempTuple.Item2 = Convert.ToInt32(ship.Substring(1,1));
            Trace.WriteLine("ExtractTuplesFromOneShip: " + tempTuple);
            CountPlacements[tempTuple.Item1, tempTuple.Item2]++;
            
        }
        


    }
}
