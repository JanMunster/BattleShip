using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {

        public bool[,] ComputerShots { get; set; }
        public bool[,] HumanShips { get; set; }
        public PopUp()
        {
            InitializeComponent();
            DrawGrid();
        }

        private void DrawGrid()
        {

            for (int i = 0; i < 10; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Name = "Column" + i.ToString();
                shotGrid.ColumnDefinitions.Add(gridCol);
            }

            for (int i = 0; i < 10; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Name = "Row" + i.ToString();
                shotGrid.RowDefinitions.Add(gridRow);
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
                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);
                    shotGrid.Children.Add(button);
                }
            }


        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int x = 0;
            int y = 0;
            foreach (Button button in shotGrid.Children)
            {
                if (ComputerShots[x, y] == true)
                {
                    button.Content = "x" ;
                    if(HumanShips[x,y] == true)
                    {
                        button.Content = "*";
                        button.Foreground = Brushes.Red;
                    }
                }
                x++;
                if (x % 10 == 0)
                {
                    x = 0;
                    y++;
                }
            }

        }
    }
}
