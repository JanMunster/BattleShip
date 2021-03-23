using BattleShipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BattleShipLibrary.ExtensionMethods;

namespace BattleShipLibrary.GameInit

{
    public class GameInit
    {
        public void PopulatePlayers(string name, bool IsAIon, bool IsHumanStarting, PlayerModel human, PlayerModel computer)
        {
            human.Name = name;
            human.IsHumanStarting = IsHumanStarting;
            human.IsAIon = IsAIon;
            PopulateHumanShips(human);
            human.ShotFired = PopulateShotsFired();
            

            computer.Name = "Computer Player";
            computer.IsHumanStarting = IsHumanStarting;
            computer.IsAIon = IsAIon;
            PopulateComputerShips(computer);
            computer.ShotFired = PopulateShotsFired();
            
        }
        private void SetOrientation(ShipModel ship)
        {
            Random random = new Random();
            int rand = random.Next(0, 2);
            Trace.WriteLine("Random number: " + rand);
            if (rand == 1)
            {
                ship.IsVertical = true;
            }
            else
            {
                ship.IsVertical = false;
            }
        }
        private void PopulateHumanShips(PlayerModel human)
        {
            List<ShipModel> ships = human.Ships;

            for (int i = 0; i < 5; i++)
            {
                ships.Add(new ShipModel());
                ships[i].IsAlive = true; // They start out alive
                ships[i].IsVertical = false; // They start out horizontal
            }

            ships[0].ShipSectionStatus = new List<string> { "O", "O", "O", "O", "O" }; // Carrier
            ships[0].ShipSize = 5;
            ships[0].ShipType = "Carrier";
            SetOrientation(ships[0]);
            GetRandomPosition(ships[0], human.ShipSectionHere);
            
            ships[1].ShipSectionStatus = new List<string> { "O", "O", "O", "O" }; // Battleship
            ships[1].ShipSize = 4;
            ships[1].ShipType = "Battleship";
            SetOrientation(ships[1]);
            GetRandomPosition(ships[1], human.ShipSectionHere);            

            ships[2].ShipSectionStatus = new List<string> { "O", "O", "O" }; // Cruiser
            ships[2].ShipSize = 3;
            ships[2].ShipType = "Cruiser";
            SetOrientation(ships[2]);
            GetRandomPosition(ships[2], human.ShipSectionHere);
            
            ships[3].ShipSectionStatus = new List<string> { "O", "O", "O" }; // Submarine
            ships[3].ShipSize = 3;
            ships[3].ShipType = "Submarine";
            SetOrientation(ships[3]);
            GetRandomPosition(ships[3], human.ShipSectionHere);
            
            ships[4].ShipSectionStatus = new List<string> { "O", "O" }; // Destroyer
            ships[4].ShipSize = 2;
            ships[4].ShipType = "Destroyer";
            SetOrientation(ships[4]);
            GetRandomPosition(ships[4], human.ShipSectionHere);
            
            for (int i = 0; i < 5; i++) // Iterate the 5 ships
            {
                foreach ((int, int) shipSection in human.Ships[i].Placement) // Iterate each ship.placement
                {
                    // Mark true in shipSectionHere for each shipsection
                    human.ShipSectionHere[shipSection.Item1, shipSection.Item2] = true;
                }
            }
        }
        private void PopulateComputerShips(PlayerModel computer)
        {
            List<ShipModel> ships = computer.Ships;

            for (int i = 0; i < 5; i++) // Add 5 new shipsModels to ships
            {
                ships.Add(new ShipModel());
                ships[i].IsAlive = true; // They start out alive                
            }

            ships[0].ShipSectionStatus = new List<string> { "O", "O", "O", "O", "O" }; // Carrier
            ships[0].ShipSize = 5;
            ships[0].ShipType = "Carrier";
            SetOrientation(ships[0]);
            GetRandomPosition(ships[0], computer.ShipSectionHere);

            ships[1].ShipSectionStatus = new List<string> { "O", "O", "O", "O" }; // Battleship
            ships[1].ShipSize = 4;
            ships[1].ShipType = "Battleship";
            SetOrientation(ships[1]);
            GetRandomPosition(ships[1], computer.ShipSectionHere);

            ships[2].ShipSectionStatus = new List<string> { "O", "O", "O" }; // Cruiser
            ships[2].ShipSize = 3;
            ships[2].ShipType = "Cruiser";
            SetOrientation(ships[2]);
            GetRandomPosition(ships[2], computer.ShipSectionHere);

            ships[3].ShipSectionStatus = new List<string> { "O", "O", "O" }; // Submarine
            ships[3].ShipSize = 3;
            ships[3].ShipType = "Submarine";
            SetOrientation(ships[3]);
            GetRandomPosition(ships[3], computer.ShipSectionHere);

            ships[4].ShipSectionStatus = new List<string> { "O", "O" }; // Destroyer
            ships[4].ShipSize = 2;
            ships[4].ShipType = "Destroyer";
            SetOrientation(ships[4]);
            GetRandomPosition(ships[4], computer.ShipSectionHere);
        }
        private void GetRandomPosition(ShipModel ship, bool[,] shipSectionHere)
        {
            List<(int, int)> tempShipPosition; // To hold a temporary random ship while checking it

            bool shipWithinGrid = false; // Will check if the random ship is placed within a 10x10 grid
            bool OverlapOtherShips = false; // Will check if the random ship overlaps already placed ships

            int x = 0; // x-coordinate for random ship section
            int y = 0; // y-coordinate for random ship section

            do // Stay in loop until random ship is within grid and does not overlap other ships
            {
                tempShipPosition = new List<(int, int)>();
                Random rand = new Random();
                x = rand.Next(0, 10); // Random integer between 0 and 9
                y = rand.Next(0, 10); // Random integer between 0 and 9
                shipWithinGrid = false; // Assume ship is not inside grid
                OverlapOtherShips = false; // Assume ship is not overlapping other ships

                if (ship.IsVertical == true) // If the ship is vertical...
                {
                    if ((y + ship.ShipSize) <= 10) // If y+size is not more than 10, the ship fits the grid
                    {
                        shipWithinGrid = true;
                        for (int i = 0; i < ship.ShipSize; i++) // Create a temporary ship
                        {
                            tempShipPosition.Add((x, y + i)); // Add vertical ship sections
                        }
                    }
                }
                else // If the ship is horizontal...
                {
                    if ((x + ship.ShipSize) <= 10)
                    {
                        shipWithinGrid = true;
                        for (int i = 0; i < ship.ShipSize; i++) // Create a temporary ship
                        {
                            tempShipPosition.Add((x + i, y)); // Add horizontal ship sections
                        }
                    }
                }

                if (shipWithinGrid == false)
                {
                    Trace.WriteLine($"{ship.ShipType} outside grid.");
                    continue;
                }

                foreach ((int, int) section in tempShipPosition)// Check if tempShip has sections on top of other sections
                {
                    if (shipSectionHere[section.Item1, section.Item2] == true)
                    {
                        OverlapOtherShips = true;
                    }
                }

                if (OverlapOtherShips == true)
                {
                    Trace.WriteLine($"{ship.ShipType} overlaps with other ship.");
                }

            } while (shipWithinGrid == false || OverlapOtherShips == true);

            ship.Placement = tempShipPosition; // Upgrade the temporary ship placements to a real ship.Placement

            foreach ((int, int) section in ship.Placement) // Update shipSectionHere with trues
            {
                shipSectionHere[section.Item1, section.Item2] = true;
            }
        }
        public bool[,] PopulateShotsFired()
        {
            bool[,] grid = new bool[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int u = 0; u < 10; u++)
                {
                    grid[i, u] = false;
                }
            }
            return grid;
        }
    }

}
