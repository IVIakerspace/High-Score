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
        public PlayersNameDialog(string s)
        {
            
            InitializeComponent();
            lblQuestion.Content = s;

        }
        
        public string PlayerNum { get; set; }
        public string Name { get; set; }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            

            Name = txtAnswer.Text;

            this.Close();

        }
    }


}
