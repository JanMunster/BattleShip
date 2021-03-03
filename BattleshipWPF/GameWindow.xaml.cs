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


namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public PlayerModel Human { get; set; }
        public PlayerModel Computer { get; set; }
        public TimeSpan FromBeginning { get; set; } = new TimeSpan(0, 0, 0);
        public bool IsAHit { get; set; } = false;
        public SolidColorBrush red = new SolidColorBrush(Color.FromRgb(250, 0, 0));
        public (int, int) ButtonPos { get; set; }
        public Button ButtonClicked { get; set; }
        public bool HumansTurn { get; set; }
        public bool IsAIon { get; set; }
        public List<(int, int)> ListOfShots { get; set; } 
        public GameWindow(PlayerModel hum, PlayerModel com, bool humanStarts, bool aIon)
        {
            Human = hum;
            Computer = com;
            HumansTurn = humanStarts;
            IsAIon = aIon;
            InitializeComponent();
            DrawGrid();
            StartVideos();
            if(HumansTurn == false)
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

            Trace.WriteLine("Shot fired at: " + ButtonPos);
        }

        private void CheckHitOrMiss()
        {
            int x = ButtonPos.Item1;
            int y = ButtonPos.Item2;
            IsAHit = false;
            Human.ShotFired[x, y] = true;

            foreach (ShipModel ship in Computer.Ships)
            {
                if (ship.Placement.Contains((x, y)))
                {
                    IsAHit = true;
                    ButtonClicked.Content = "*";
                    ButtonClicked.Foreground = red;
                    ButtonClicked.FontSize = 42;
                    CheckShipAlive(ship);
                    Trace.WriteLine("Hit: " + ship.ShipType + " " + (x, y));
                    UpdateShipSectionStatus(x, y, ship);
                    break;
                }
            }

            if (IsAHit == false)
            {
                Trace.WriteLine("Miss: " + (x, y));
                ButtonClicked.Content = "X";
            }
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

        private void Humanfire_MediaEnded(object sender, RoutedEventArgs e)
        {
            humanfiringvideo.MediaEnded -= Humanfire_MediaEnded;
            humanfiringvideo.Stop();
            humanfiringvideo.Visibility = Visibility.Hidden;
            navyfootage.Visibility = Visibility.Visible;
            CheckHitOrMiss();
            //Trace.Write("Before call to DisplayHitMiss  ");
            DisplayHitMissText();
            //Trace.Write("After call to DisplayHitMiss  ");
        }

        private void computerfiringvideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            computerfiringvideo.Stop();
            computerfiringvideo.Visibility = Visibility.Hidden;
            navyfootage2.Visibility = Visibility.Visible;


        }

        private void DisplayHitMissText()
        {
            HitMiss.Visibility = Visibility.Visible;
            if (IsAHit == true)
            {
                HitMiss.Text = "HIT!";
            }
            else
            {
                HitMiss.Text = "MISS!";
            }

            IsAHit = false;
            //Trace.WriteLine("Before delay");
            Task.Delay(2000).ContinueWith(_ =>
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

        private void OpponentsTurn()
        {
            Trace.WriteLine("Now opponents turn.");
            GameGrid.IsHitTestVisible = true;
            (int, int) nextShot = getNextShot();
        }

        private (int, int) getNextShot()
        {
            if (ListOfShots == null)
            {
                GenerateListOfShots();
            }

                return (-1, -1);
        }

        private void GenerateListOfShots()
        {            
            ListOfShots = new List<(int, int)>();            
            Trace.WriteLine("AI is " + IsAIon);
            for (int y = 0; y < 10; y++)
            {
                for (int x  = 0; x < 10; x++)
                {
                    ListOfShots.Add((x,y));
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
            humanfiringvideo.MediaEnded += Humanfire_MediaEnded;
        }
    }
}
