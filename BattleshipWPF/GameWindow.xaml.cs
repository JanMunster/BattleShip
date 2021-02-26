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
using BattleShipLibrary.Models;


namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public PlayerModel Human{ get; set; }
        public PlayerModel Computer { get; set; }
        public GameWindow(PlayerModel hum, PlayerModel com)
        {
            Human = hum;
            Computer = com;
            InitializeComponent();
        }
    }
}
