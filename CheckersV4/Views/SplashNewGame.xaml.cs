using CheckersV4.Models;
using CheckersV4.Services;
using CheckersV4.ViewModels;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CheckersV4.Views
{
    /// <summary>
    /// Interaction logic for SplashNewGame.xaml
    /// </summary>
    public partial class SplashNewGame : Window
    {
        public SplashNewGame()
        {
            InitializeComponent();    
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            BoardVM.Player1.Name = Player1TextBox.Text;
            BoardVM.Player2.Name = Player2TextBox.Text;
           
            MainWindow mW = new MainWindow();
            mW.Show();
            this.Close();
        }
    }
}
