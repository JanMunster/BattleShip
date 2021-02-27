using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            navyfootage.Pause();
            navyfootage.Visibility = Visibility.Hidden;
            humanfiringvideo.Visibility = Visibility.Visible;
            humanfiringvideo.Play();
            humanfiringvideo.MediaEnded += Humanfire_MediaEnded;
        }

        private void Humanfire_MediaEnded(object sender, RoutedEventArgs e)
        {
            humanfiringvideo.Visibility = Visibility.Collapsed;
            navyfootage.Visibility = Visibility.Visible;
            navyfootage.Play();
        }
    }
}
