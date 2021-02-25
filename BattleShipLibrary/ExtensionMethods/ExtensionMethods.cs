using System;
using System.Collections.Generic;
using System.Text;
using BattleShipLibrary.Models;
using System.Diagnostics;


namespace BattleShipLibrary.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static void PrintPlayer(this PlayerModel player)
        {

            Trace.WriteLine("Player name: " + player.Name);
            Trace.WriteLine("Player is starting player: " + player.IsHumanStarting);
            Trace.WriteLine("ships:");

            for (int t = 0; t < 5; t++) // Loops through all ships of the player
            {
                Trace.Write($"{player.Ships[t].ShipType} section status: ");
                for (int i = 0; i < player.Ships[t].ShipSize; i++) // Loops through sectionms of a single ship
                {
                    Trace.Write(player.Ships[t].ShipSectionStatus[i]);
                }
                Trace.Write("  is alive: " + player.Ships[t].IsAlive);
                Trace.WriteLine("  is vertical: " + player.Ships[t].IsVertical);
            }

            Trace.WriteLine("Ship grid using ship.placement");
            bool shipPresent = false;
            for (int i = 0; i < 10; i++)
            {
                for (int u = 0; u < 10; u++)
                {
                    foreach (var ship in player.Ships)
                    {
                        for (int t = 0; t < ship.ShipSize; t++)
                        {
                            if (ship.Placement[t] == (u, i))
                            {
                                shipPresent = true;
                            }
                        }
                    }
                    if (shipPresent == true)
                    {
                        Trace.Write(" O");
                        shipPresent = false;
                    }
                    else
                    {
                        Trace.Write(" .");
                    }
                }
                Trace.WriteLine(" ");
            }
        }

        public static void PrintShotsFired(this PlayerModel player)
        {
            Trace.WriteLine($"Shots fired from {player.Name}:");
            for (int i = 0; i < 10; i++)
            {
                for (int u = 0; u < 10; u++)
                {
                    if (player.ShotFired[i, u] == true)
                    {
                        Trace.Write(" *");
                    }
                    else
                    {
                        Trace.Write(" .");
                    }
                }
                Trace.WriteLine(" ");
            }
        }

        public static void PrintShipSections(this PlayerModel player)
        {
            Trace.WriteLine($"Ship grid for {player.Name} using ship.shipSectionStatus:");
            for (int i = 0; i < 10; i++)
            {
                for (int u = 0; u < 10; u++)
                {
                    if (player.ShipSectionHere[u, i] == true)
                    {
                        Trace.Write(" S");
                    }
                    else
                    {
                        Trace.Write(" -");
                    }
                }
                Trace.WriteLine(" ");
            }
        }

    }
}

