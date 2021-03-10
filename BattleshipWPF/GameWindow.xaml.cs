using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BattleShipLibrary.Models;
using BattleShipLibrary.ExtensionMethods;
using System.Windows.Controls.Primitives;

namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private const int TIMEDELAY = 1000;
        public PlayerModel Human { get; set; }
        public PlayerModel Computer { get; set; }
        public TimeSpan FromBeginning { get; set; } = new TimeSpan(0, 0, 0);
        public bool HumanHit { get; set; } = false;
        public bool ComputerHit { get; set; } = false;
        public bool LastComputerShotWasHit { get; private set; }

        public PopUp popUp = new PopUp();

        public SolidColorBrush red = new SolidColorBrush(Color.FromRgb(250, 0, 0));
        public (int, int) ButtonPos { get; set; }
        public Button ButtonClicked { get; set; }
        public bool HumansTurn { get; set; }
        public bool IsAIon { get; set; }
        public List<(int, int)> ListOfShots { get; set; }
        public List<(int, int)> ListOfPriorityShots { get; set; } = new List<(int, int)>();
        public (int, int) ComputerShot { get; set; }
        public string RememberButtonContent { get; set; } = " ";
        public SolidColorBrush RememberButtonColor { get; set; } = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        public GameWindow(PlayerModel hum, PlayerModel com, bool humanStarts, bool aIon)
        {
            Human = hum;
            Computer = com;
            HumansTurn = humanStarts;
            IsAIon = aIon;
            InitializeComponent();
            DrawGrid();
            StartVideos();

            if (HumansTurn == false)
            {
                GameGrid.IsHitTestVisible = false;
                OpponentsTurn();
            }
        }

        private void StartVideos()
        {
            navyfootage.Play();
            navyfootage2.Play();
        }

        private void DrawGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Name = "Column" + i.ToString();
                GameGrid.ColumnDefinitions.Add(gridCol);
            }

            for (int i = 0; i < 10; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Name = "Row" + i.ToString();
                GameGrid.RowDefinitions.Add(gridRow);
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
                    button.Content = "";
                    button.AddHandler(Button.ClickEvent, new RoutedEventHandler(GameClick));
                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);
                    GameGrid.Children.Add(button);
                }
            }
        }

        private void GameClick(object sender, RoutedEventArgs e)
        {
            GameGrid.IsHitTestVisible = false;
            ButtonPos = ((int, int))(sender as Button).Tag;
            ButtonClicked = (sender as Button);
            if (Human.ShotFired[ButtonPos.Item1, ButtonPos.Item2] == true)
            {
                return;
            }
            FireShot.Visibility = Visibility.Visible;

            ShowHumanFireVideo();

            Trace.WriteLine("Human Shot fired at: " + ButtonPos);
        }

        private void CheckShipAlive(ShipModel ship)
        {
            if (ship.ShipSectionStatus.Contains("O") == false)
            {
                ship.IsAlive = false;
                Trace.WriteLine("Ship not alive");
            }
        }

        private void UpdateShipSectionStatus(int x, int y, ShipModel ship)
        {
            for (int i = 0; i < ship.ShipSize; i++)
            {
                if (ship.Placement[i] == (x, y))
                {
                    Trace.WriteLine("ShipSections before: " + DisplaySections(ship.ShipSectionStatus));
                    ship.ShipSectionStatus[i] = "X";
                    Trace.WriteLine("ShipSections before: " + DisplaySections(ship.ShipSectionStatus));
                }
            }
        }

        private string DisplaySections(List<string> shipSectionStatus)
        {
            string sectionsAsSingleString = "";
            foreach (string section in shipSectionStatus)
            {
                sectionsAsSingleString += section;
            }
            return sectionsAsSingleString;
        }

        private void DisplayHumanHitMissText()
        {
            HitMiss.Visibility = Visibility.Visible;
            if (HumanHit == true)
            {
                HitMiss.Text = "HIT!";
            }
            else
            {
                HitMiss.Text = "MISS!";
            }

            HumanHit = false;
            //Trace.WriteLine("Before delay");
            Task.Delay(TIMEDELAY).ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    HitMiss.Visibility = Visibility.Hidden;
                    FireShot.Visibility = Visibility.Hidden;
                    Trace.WriteLine("After delay");
                    OpponentsTurn();
                });
            });
        }

        private void DisplayComputerHitMissText()
        {
            computerHitMissText.Visibility = Visibility.Visible;
            if (ComputerHit == true)
            {
                computerHitMissText.Text = "HIT!";
                updateHumanShipDisplay();
            }
            else
            {
                computerHitMissText.Text = "MISS!";
            }

            ComputerHit = false;
            Task.Delay(TIMEDELAY).ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    GameGrid.IsHitTestVisible = true;
                    CheckForComputerWinner();
                    SwitchToHumanTurnTextUpdate();
                    Trace.WriteLine("After computers turn delay");
                    int x = ComputerShot.Item1;
                    int y = ComputerShot.Item2 * 10;
                    (GameGrid.Children[x + y] as Button).Content = RememberButtonContent;
                    (GameGrid.Children[x + y] as Button).Foreground = RememberButtonColor;
                    GameGrid.IsHitTestVisible = true;
                });
            });
        }

        private void updateHumanShipDisplay()
        {
            carrierStatus.Text = DisplaySections(Human.Ships[0].ShipSectionStatus);
            battleshipStatus.Text = DisplaySections(Human.Ships[1].ShipSectionStatus);
            cruiserStatus.Text = DisplaySections(Human.Ships[2].ShipSectionStatus);
            submarineStatus.Text = DisplaySections(Human.Ships[3].ShipSectionStatus);
            destroyerStatus.Text = DisplaySections(Human.Ships[4].ShipSectionStatus);

        }

        private void SwitchToHumanTurnTextUpdate()
        {
            computerHitMissText.Visibility = Visibility.Hidden;
            computerOpponentText.Visibility = Visibility.Hidden;
            computerTurnText.Visibility = Visibility.Hidden;
            computerFireText.Visibility = Visibility.Hidden;

            yourTurnText.Visibility = Visibility.Visible;
            clickGridToFireText.Visibility = Visibility.Visible;

            (GameGrid.Children[ComputerShot.Item1 + ComputerShot.Item2 * 10] as Button).Content = RememberButtonContent;
            (GameGrid.Children[ComputerShot.Item1 + ComputerShot.Item2 * 10] as Button).Foreground = RememberButtonColor;
            GameGrid.IsHitTestVisible = true;
        }

        private void CheckComputerHitOrMiss()
        {
            int x = ComputerShot.Item1;
            int y = ComputerShot.Item2;
            ComputerHit = false;
            Computer.ShotFired[x, y] = true;

            foreach (ShipModel ship in Human.Ships)
            {
                if (ship.Placement.Contains((x, y)))
                {
                    ComputerHit = true;
                    LastComputerShotWasHit = true;
                    UpdatePriorityList(x, y);
                    UpdateShipSectionStatus(x, y, ship);
                    CheckShipAlive(ship);
                    Trace.WriteLine("Computer Hit: " + ship.ShipType + " " + (x, y));
                    (GameGrid.Children[x + y * 10] as Button).Content = "H";
                    (GameGrid.Children[x + y * 10] as Button).Foreground = red;
                    break;
                }
            }

            if (ComputerHit == false)
            {
                Trace.WriteLine("Computer Miss: " + (x, y));
                (GameGrid.Children[x + y * 10] as Button).Content = "M";
                (GameGrid.Children[x + y * 10] as Button).Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }


       
        private void UpdatePriorityList(int x, int y)
        {
            List<(int, int)> possibleShots = new List<(int, int)>();
            possibleShots.Add((x - 1, y)); // Add position left of shot
            possibleShots.Add((x, y - 1)); // Add position above shot
            possibleShots.Add((x + 1, y)); // Add position right of shot
            possibleShots.Add((x, y + 1)); // Add position below of shot           

            foreach ((int, int) shot in possibleShots)
            {
                if (shot.Item1 > 9 || shot.Item1 < 0 || shot.Item2 > 9 || shot.Item2 < 0)
                {
                    continue; // Examine next shot, if shot is outside game grid
                }

                if (Computer.ShotFired[shot.Item1, shot.Item2] == true)
                {
                    continue; // Examine next shot, if computer fired here previously
                }

                if (ListOfPriorityShots.Contains(shot))
                {
                    continue; // If shot is already in ListOfPriorityShots, continue without adding
                }

                ListOfPriorityShots.Add(shot);
            }
        }


        private void OpponentsTurn()
        {
            CheckForHumanWinner();
            Trace.WriteLine("Now opponents turn.");
            OpponentTurnUpdateTexts();
            ComputerShot = getNextShot();
            ListOfShots.PrintListOfShots();
            Trace.WriteLine("\nComputer shot fired at: " + ComputerShot);
            ShowComputerFireVideo();
        }

        private void OpponentTurnUpdateTexts()
        {
            computerTurnText.Visibility = Visibility.Visible;
            computerOpponentText.Visibility = Visibility.Visible;
            computerFireText.Visibility = Visibility.Visible;
            yourTurnText.Visibility = Visibility.Hidden;
            clickGridToFireText.Visibility = Visibility.Hidden;
        }        

        private void Humanfire_MediaEnded(object sender, RoutedEventArgs e)
        {
            humanfiringvideo.Pause();
            humanfiringvideo.Position = TimeSpan.FromMilliseconds(1);
            humanfiringvideo.Visibility = Visibility.Hidden;
            navyfootage.Visibility = Visibility.Visible;
            CheckHumanHitOrMiss();
            DisplayHumanHitMissText();
        }

        private void Computerfiringvideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            computerfiringvideo.Pause();
            computerfiringvideo.Position = TimeSpan.FromMilliseconds(1);
            computerfiringvideo.Visibility = Visibility.Hidden;
            navyfootage2.Visibility = Visibility.Visible;
            CheckComputerHitOrMiss();
            DisplayComputerHitMissText();
        }

        private void CheckHumanHitOrMiss()
        {
            int x = ButtonPos.Item1;
            int y = ButtonPos.Item2;
            HumanHit = false;
            Human.ShotFired[x, y] = true;

            foreach (ShipModel ship in Computer.Ships)
            {
                if (ship.Placement.Contains((x, y)))
                {
                    HumanHit = true;
                    ButtonClicked.Content += "*";
                    ButtonClicked.Foreground = red;
                    ButtonClicked.FontSize = 42;
                    UpdateShipSectionStatus(x, y, ship);
                    CheckShipAlive(ship);
                    Trace.WriteLine("Hit: " + ship.ShipType + " " + (x, y));
                    break;
                }
            }

            if (HumanHit == false)
            {
                Trace.WriteLine("Human miss: " + (x, y));
                ButtonClicked.Content += "X";
            }
        }

        private (int, int) getNextShot()
        {
            (int, int) shot = (0, 0);

            if (ListOfShots == null) // If no list of shots generated, create one. 
            {
                GenerateListOfShots();
            }

            if (ListOfPriorityShots.Count == 0)
            {
                shot = GetShotFromList();
            }
            else
            {
                shot = GetShotFromPriorityList();
            }

            StoreButtonInfo(shot);

            return shot; // Return the copied shot
        }

        private (int, int) GetShotFromPriorityList()
        {
            (int, int) shot = ListOfPriorityShots[ListOfPriorityShots.Count - 1]; // Copy last coordinate in ListOfPriotityShots
            ListOfPriorityShots.RemoveAt(ListOfPriorityShots.Count - 1); // Remove last coordinate in ListOfPriorityShots
            ListOfShots.Remove(shot); // Also remove from ListOfShots
            return shot;
        }

        private (int, int) GetShotFromList()
        {
            (int, int) shot = ListOfShots[ListOfShots.Count - 1]; // Copy last coordinate in ListOfShots
            ListOfShots.RemoveAt(ListOfShots.Count - 1); // Remove last coordinate in ListOfShots
            return shot;
        }

        private void StoreButtonInfo((int, int) shot)
        {
            int x = shot.Item1;
            int y = shot.Item2;
            RememberButtonContent = (GameGrid.Children[x + (y * 10)] as Button).Content.ToString();
            RememberButtonColor = ((SolidColorBrush)(GameGrid.Children[x + (y * 10)] as Button).Foreground);
            Trace.WriteLine("string saved: '" + RememberButtonContent + "'");
            Trace.WriteLine("Color saved: '" + RememberButtonColor.ToString() + "'");
        }

        private void GenerateListOfShots()
        {
            ListOfShots = new List<(int, int)>();
            Trace.WriteLine("AI is " + IsAIon);
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    ListOfShots.Add((x, y));
                }
            }

            ListOfShots.PrintListOfShots();
            ListOfShots = ListOfShots.OrderBy(x => Guid.NewGuid()).ToList();
            ListOfShots.PrintListOfShots();
        }

        private void ShowHumanFireVideo()
        {
            navyfootage.Visibility = Visibility.Hidden;
            humanfiringvideo.Visibility = Visibility.Visible;
            humanfiringvideo.Play();
        }

        private void ShowComputerFireVideo()
        {
            navyfootage2.Visibility = Visibility.Hidden;
            computerfiringvideo.Visibility = Visibility.Visible;
            computerfiringvideo.Play();
        }

        private void CheckForHumanWinner()
        {
            bool humanWon = true;

            foreach (ShipModel ship in Computer.Ships)
            {
                if (ship.IsAlive == true)
                {
                    humanWon = false;
                }
            }

            if (humanWon == true)
            {
                Trace.WriteLine("HUMAN WON!");
                ShowEndScreen(Human.ShotFired, 1); // 1 denotes human winner
            }
        }

        private void CheckForComputerWinner()
        {
            bool computerWon = true;

            foreach (ShipModel ship in Human.Ships)
            {
                if (ship.IsAlive == true)
                {
                    computerWon = false;
                }
            }

            if (computerWon == true)
            {
                Trace.WriteLine("COMPUTER WON!");
                ShowEndScreen(Computer.ShotFired, 0); // 0 denotes computer winner
            }
        }

        private void ShowEndScreen(bool[,] shotFired, int winner)
        {
            int totalShots = 0;
            foreach (bool shot in shotFired) // Count all shots fired by player
            {
                if (shot == true)
                {
                    totalShots++;
                }
            }

            EndScreen endScreen = new EndScreen();
            endScreen.ShotsToFinish = totalShots;
            endScreen.Show();

            if (winner == 1)
            {
                endScreen.LostImage.Visibility = Visibility.Hidden;
                endScreen.wonImage.Visibility = Visibility.Visible;
                endScreen.wonOrLostText.Text = "YOU WON!";
                endScreen.numberOfShotsText.Text = "You used " + totalShots +
                    " shots to destroy the opponent.";
            }
            else
            {
                endScreen.LostImage.Visibility = Visibility.Visible;
                endScreen.wonImage.Visibility = Visibility.Hidden;
                endScreen.wonOrLostText.Text = "YOU LOST!";
                endScreen.numberOfShotsText.Text = "The opponent took " + totalShots +
                    " shots to sink your ships.";
            }

            this.Close();
        }

        private void navyfootage2_MediaEnded(object sender, RoutedEventArgs e)
        {
            navyfootage2.Position = TimeSpan.FromMilliseconds(1);
            navyfootage2.Play();
        }

        private void navyfootage_MediaEnded(object sender, RoutedEventArgs e)
        {
            navyfootage.Position = TimeSpan.FromMilliseconds(1);
            navyfootage.Play();
        }

        private void but_MouseEnter(object sender, MouseEventArgs e)
        {
            popUp.ComputerShots = Computer.ShotFired;
            popUp.HumanShips = Human.ShipSectionHere;

            if (popUp.IsVisible == false)
            {
                popUp.Show();
            } else
            {
                popUp.Visibility = Visibility.Visible;
            }            
        }        

        private void but_MouseLeave(object sender, MouseEventArgs e)
        {
            popUp.Visibility = Visibility.Hidden;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            popUp.Close();
        }
    }
}
