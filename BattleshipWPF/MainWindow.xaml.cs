using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using BattleShipLibrary.Models;
using BattleShipLibrary.ExtensionMethods;
using BattleShipLibrary.GameInit;

namespace BattleshipWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleAI_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleAI.IsChecked == true)
            {
                AItext.Text = "(AI will use previous games to decide tactics)";
                Trace.WriteLine(ToggleAI.IsChecked);
            }
            else
            {
                AItext.Text = "(AI will randomize its shots)";
                Trace.WriteLine(ToggleAI.IsChecked);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameInit gameInit = new GameInit(); // Class that holds all player initialization routines
            PlayerModel human = new PlayerModel(); // New human player
            PlayerModel computer = new PlayerModel(); // New computer player

            string playerName = PlayerName.Text; // Capture name from WPF window
            bool AIon = (bool)ToggleAI.IsChecked; // Capture AI on/off from WPF window 
            bool humanStarts = (bool)ToggleStartingPLayer.IsChecked; // Capture who starts from WPF window

            gameInit.PopulatePlayers(playerName, AIon, humanStarts, human, computer); // Init players

            human.PrintPlayer(); // Extension methods that write debug info to console
            human.PrintShotsFired();
            human.PrintShipSections();

            computer.PrintPlayer();
            computer.PrintShotsFired();
            computer.PrintShipSections();

            ShipPlacement shipPlacement = new ShipPlacement(human,computer);            
            shipPlacement.Show();
            this.Close();
        }








    }
}

