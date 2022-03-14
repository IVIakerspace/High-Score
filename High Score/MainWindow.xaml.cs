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

namespace High_Score
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

        private void OnClickEsc(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to exit?";
            string caption = "Exit";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                // OK code here  
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                // Cancel code here  
            }
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            PlayersNameDialog playersNameDialog = new PlayersNameDialog();
            playersNameDialog.ShowDialog();

        }

        private void LoadHighScooreList()
        {

        }
    }
}


