using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public GameWindow(PlayerModel hum, PlayerModel com)

        {
            Human = hum;
            Computer = com;
            InitializeComponent();
            DrawGrid();
            StartVideos();
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
            (int, int) buttonPos = ((int, int))(sender as Button).Tag;
            if (Human.ShotFired[buttonPos.Item1, buttonPos.Item2] == true)
            {
                return;
            }

            ShowHumanFireVideo();

            FireShot.Visibility = Visibility.Visible;
            Trace.WriteLine("Shot fired at: " + buttonPos);

            CheckHitOrMiss(buttonPos, (sender as Button));


        }

        private void CheckHitOrMiss((int, int) pos, Button button)
        {
            int x = pos.Item1;
            int y = pos.Item2;

            foreach (ShipModel ship in Computer.Ships)
            {
                if (ship.Placement.Contains((x, y)))
                {
                    Trace.WriteLine("Hit: " + ship.ShipType + " " + (x, y));
                    UpdateShipSectionStatus(x, y, ship);
                    CheckShipAlive(ship);
                    IsAHit = true;
                    button.Content = "*";
                    button.Foreground = red;
                }
                else
                {
                    Trace.WriteLine("Miss: " + (x, y));
                    IsAHit = false;
                    button.Content = "X";
                }
                Human.ShotFired[x, y] = true;
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
            humanfiringvideo.Stop();
            humanfiringvideo.Visibility = Visibility.Hidden;
            navyfootage.Visibility = Visibility.Visible;
            Trace.Write("Before call to DisplayHitMiss  ");
            DisplayHitMiss();
            Trace.Write("After call to DisplayHitMiss  ");
        }
        private void computerfiringvideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            computerfiringvideo.Stop();
            computerfiringvideo.Visibility = Visibility.Hidden;
            navyfootage2.Visibility = Visibility.Visible;


        }

        private void DisplayHitMiss()
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
            Trace.WriteLine("Before delay");
            Task.Delay(2000).ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    HitMiss.Visibility = Visibility.Hidden;
                    FireShot.Visibility = Visibility.Hidden;
                });
            });
            Trace.WriteLine("After delay");

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
