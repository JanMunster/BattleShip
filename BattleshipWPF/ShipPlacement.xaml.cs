using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BattleShipLibrary.Models;
using BattleShipLibrary.GameInit;
using BattleShipLibrary.ExtensionMethods;


namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : Window
    {
        public PlayerModel human;
        public PlayerModel computer;
        private int shipSelected = 0;
        SolidColorBrush gray = new SolidColorBrush(Color.FromRgb(0xD3, 0xD3, 0xD3));
        private GameInit gameInit = new GameInit();
        public bool HumanStarts { get; set; }
        public bool AIon { get; set; }

        public ShipPlacement(PlayerModel hum, PlayerModel com, bool humStarts, bool aIon)
        {
            human = hum;
            computer = com;
            HumanStarts = humStarts;
            AIon = aIon;
            InitializeComponent();
            DrawGrid();
            UpdateGrid();
        }

        private void ShipClick(object sender, RoutedEventArgs e)
        {
            Carrier.Background = gray;
            Battleship.Background = gray;
            Cruiser.Background = gray;
            Submarine.Background = gray;
            Destroyer.Background = gray;

            (sender as Button).Background = new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0x90));
            shipSelected = Int32.Parse((sender as Button).Tag.ToString());
            Trace.WriteLine("Ship selected: " + shipSelected);
            UpdateGrid();
        }
        private void GridClick(object sender, RoutedEventArgs e)
        {
            bool fitsInGrid = false;
            bool overlapsOtherShips = true;

            (int, int) buttonPos = ((int, int))(sender as Button).Tag;
            List<(int, int)> newShip = new List<(int, int)>();

            CalculateNewShip(newShip, buttonPos);
            fitsInGrid = CheckFitInGrid(buttonPos);
            if (fitsInGrid == true)
            {
                overlapsOtherShips = CheckOverlap(newShip);
            }

            if (fitsInGrid == true && overlapsOtherShips == false)
            {
                moveShip(newShip, human.Ships[shipSelected].Placement);
            }
        }
        private void RandomizeShipsClick(object sender, RoutedEventArgs e)
        {
            string playerName = human.Name;
            bool isAIon = human.IsAIon;
            bool isHumanStarting = human.IsHumanStarting;
            human = new PlayerModel();
            computer = new PlayerModel();
            gameInit.PopulatePlayers(playerName, isAIon, isHumanStarting, human, computer);
            UpdateGrid();
        }
        private void FlipClick(object sender, RoutedEventArgs e)
        {
            bool isNewPosOK = false;
            bool overlap = true;
            bool fitInGrid = false;
            Random rand = new Random();
            int x = 0;
            int y = 0;
            List<(int, int)> newShip;

            human.Ships[shipSelected].IsVertical = !human.Ships[shipSelected].IsVertical;
            do
            {
                newShip = new List<(int, int)>();
                x = rand.Next(0, 10);
                y = rand.Next(0, 10);

                //CalculateNewFlippedShip(x, y, newShip);
                CalculateNewShip(newShip, (x, y));
                fitInGrid = CheckFitInGrid((x, y));
                if (fitInGrid == true)
                {
                    overlap = CheckOverlap(newShip);
                }

                isNewPosOK = (fitInGrid == true && overlap == false);

            } while (isNewPosOK == false);

            moveShip(newShip, human.Ships[shipSelected].Placement);
            UpdateGrid();
        }       
        private void StartClick(object sender, RoutedEventArgs e)
        {
            human.PrintPlayer();
            computer.PrintPlayer();
            GameWindow gameWindow = new GameWindow(human, computer,HumanStarts,AIon);
            gameWindow.Show();
            this.Close();                    
        }

        private void DrawGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Name = "Column" + i.ToString();
                ShipGrid.ColumnDefinitions.Add(gridCol);
            }

            for (int i = 0; i < 10; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Name = "Row" + i.ToString();
                ShipGrid.RowDefinitions.Add(gridRow);
            }

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Button button = new Button();
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.Tag = (x, y);
                    button.FontSize = 32;
                    button.AddHandler(Button.ClickEvent, new RoutedEventHandler(GridClick));
                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);
                    ShipGrid.Children.Add(button);
                }
            }
        }
        private void UpdateGrid()
        {
            int x = 0;
            int y = 0;
            SolidColorBrush BlackColor = new SolidColorBrush(Colors.Black);
            SolidColorBrush YellowColor = new SolidColorBrush(Colors.Red);

            foreach (Button button in ShipGrid.Children)
            {
                button.Foreground = BlackColor;
                button.Content = ".";
                int CurrentshipType = 0;
                (int, int) buttonPos = ((int, int))button.Tag;
                x = buttonPos.Item1;
                y = buttonPos.Item2;
                foreach (ShipModel ship in human.Ships)
                {
                    if (ship.Placement.Contains((x, y)))
                    {
                        if (shipSelected == CurrentshipType)
                        {
                            button.Foreground = YellowColor;
                        }
                        else
                        {
                            button.Foreground = BlackColor;
                        }
                        button.Content = "O";
                        Trace.Write(" O" + (x, y));
                        Trace.WriteLine(button.Foreground);
                    }
                    CurrentshipType++;
                }
            }
        }
        private void moveShip(List<(int, int)> newShip, List<(int, int)> oldShip)
        {
            foreach ((int, int) oldSection in oldShip)
            {
                human.ShipSectionHere[oldSection.Item1, oldSection.Item2] = false;
            }

            human.Ships[shipSelected].Placement.Clear();
            foreach ((int, int) newSection in newShip)
            {
                human.ShipSectionHere[newSection.Item1, newSection.Item2] = true;
                human.Ships[shipSelected].Placement.Add(newSection);
            }
            UpdateGrid();
        }
        private bool CheckOverlap(List<(int, int)> newShip)
        {
            bool overlap = false;

            foreach ((int, int) section in newShip)
            {
                int newX = section.Item1;
                int newY = section.Item2;
                if (human.ShipSectionHere[newX, newY])
                {
                    if (human.Ships[shipSelected].Placement.Contains((newX, newY)))
                    {
                        continue;
                    }
                    overlap = true;
                    Trace.WriteLine("Overlap found at " + section);
                }
            }

            return overlap;
        }
        private void CalculateNewShip(List<(int, int)> newShip, (int, int) buttonPos)
        {
            int shipSize = human.Ships[shipSelected].ShipSize;
            int x = buttonPos.Item1;
            int y = buttonPos.Item2;
            Trace.WriteLine("New possible ship:");

            if (human.Ships[shipSelected].IsVertical == true)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    newShip.Add((x, y + i));
                    Trace.Write((x, y + i));
                }
            }

            if (human.Ships[shipSelected].IsVertical == false)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    newShip.Add((x + i, y));
                    Trace.Write((x + i, y));
                }
            }
        }
        private bool CheckFitInGrid((int, int) pos)
        {
            ShipModel ship = human.Ships[shipSelected];
            int shipSize = ship.ShipSize;
            bool shipWithinGrid = false;
            Trace.WriteLine(shipSize);
            int x = pos.Item1;
            int y = pos.Item2;

            if (ship.IsVertical == true) // If the ship is vertical...
            {
                if ((y + ship.ShipSize) <= 10) // If y+size is not more than 10, the ship fits the grid
                {
                    shipWithinGrid = true;
                }
            }
            else // If the ship is horizontal...
            {
                if ((x + ship.ShipSize) <= 10)
                {
                    shipWithinGrid = true;
                }
            }

            if (shipWithinGrid == false)
            {
                Trace.WriteLine("Outside grid.");
            }
            return shipWithinGrid;
        }

    }
}
