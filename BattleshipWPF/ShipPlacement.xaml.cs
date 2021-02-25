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

namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : Window
    {
        public PlayerModel human;

        public ShipPlacement(PlayerModel hum)
        {
            human = hum;
            InitializeComponent();
            DrawGrid();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            int x = 0;
            int y = 0;

            foreach (Button button in ShipGrid.Children)
            {
                if (human.ShipSectionHere[x, y] == true)
                {
                    button.Content = "O";
                }
                else
                {
                    button.Content = ".";
                }
                x++;
                if (x == 10)
                {
                    x = 0;
                    y++;
                }
            }
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

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Button button = new Button();
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.Tag = (x, y);
                    button.FontSize = 32;
                    button.AddHandler(Button.ClickEvent, new RoutedEventHandler(GridClick));
                    Grid.SetColumn(button, y);
                    Grid.SetRow(button, x);
                    ShipGrid.Children.Add(button);
                }
            }
        }

        private void GridClick(object sender, RoutedEventArgs e)
        {
            (int, int) buttonPos = ((int, int))(sender as Button).Tag;
            Trace.WriteLine($"Grid clicked: {buttonPos}");
        }

        private void ShipClick(object sender, RoutedEventArgs e)
        {
            string name = (sender as ToggleButton).Name;
            string tag = (sender as ToggleButton).Tag.ToString();
            string content = (sender as ToggleButton).Content.ToString();
            Trace.WriteLine(name);
            Trace.WriteLine(tag);
            Trace.WriteLine(content);
            Trace.WriteLine("click");

        }
    }
}
