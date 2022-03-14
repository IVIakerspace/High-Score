using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace High_Score
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PlayersNameDialog : Window
    {
        public PlayersNameDialog()
        {
            InitializeComponent();
        }

       

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            Players players = new Players();    
            players.PlayerName=txtAnswer.Text;

            //string json = JsonSerializer.Serialize(players);
            //File.WriteAllText(@"players.json", json);
            this.Close();
            
        }
    }
}
