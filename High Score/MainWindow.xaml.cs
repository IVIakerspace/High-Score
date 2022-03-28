using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
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
using System.Windows.Threading;
using System.Text.Json.Serialization;
using System.Media;

namespace High_Score
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort port = new SerialPort("COM13", 9600, Parity.None, 8, StopBits.One);

        public MainWindow()
        {
            InitializeComponent();
            LoadHighScoreList();           

            port.DataReceived += Port_DataReceived;
            try
            {
                port.Open();
                

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                
            }
                   

        }
        int _p1Score;
        int _p2Score;

        

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = port.ReadLine();
            if (data.Contains("one player game"))
            {
                onePlayerScreen();
            }
            if (data.Contains("two players game"))
            {
                twoPlayersScreen();                
            }

            if (data.Contains("time is: "))
            {
                try
                {
                    int _time = Convert.ToInt32(data.Substring(9));

                    double d = 30 - (_time / 1000.0);
                    UpdateLabelText(TimeLabel, "Time - " + d.ToString("N1", CultureInfo.CurrentCulture));

                }
                catch (Exception)
                {

                }
            }

            if (data.Contains("GameOver"))
            {
                if (_p1Score > _p2Score)
                {
                    if (_p1Score > playersList[playersList.Count - 1].Score)
                    {
                        Player p = new Player();
                        p.Score = _p1Score;
                        p.PlayerName = GetName("  Player 1 Name ");
                        SetHighScore(p);
                        
                    }
                    if (_p2Score > playersList[playersList.Count - 1].Score)
                    {
                        Player p = new Player();
                        p.Score = _p2Score;
                        p.PlayerName = GetName("  Player 2 Name ");
                        SetHighScore(p);
                        
                    }
                }
                else
                {
                    if (_p2Score > playersList[playersList.Count - 1].Score)
                    {
                        Player p = new Player();
                        p.Score = _p2Score;
                        p.PlayerName = GetName("  Player 2 Name ");
                        SetHighScore(p);
                    }
                    if (_p1Score > playersList[playersList.Count - 1].Score)
                    {
                        Player p = new Player();
                        p.Score = _p1Score;
                        p.PlayerName = GetName("  Player 1 Name ");
                        SetHighScore(p);
                    }

                }

                _p1Score = 0;
                _p2Score = 0;

                savePlayersFile();

                Thread.Sleep(2000);

                highScoreScreen();
            }
            if (data.Contains("Player 1 Score:"))
            {
                try
                {
                    _p1Score = Convert.ToInt32(data.Substring(15));
                    UpdateLabelText(P1ScoreLabel, "Player 1 Score - " + _p1Score.ToString());
                }
                catch (Exception)
                {

                }


            }
            if (data.Contains("Player 2 Score:"))
            {
                try
                {
                    _p2Score = Convert.ToInt32(data.Substring(15));
                    UpdateLabelText(P2ScoreLabel, "Player 2 Score - " + _p2Score.ToString());
                }
                catch (Exception)
                {


                }

            }
        }

        private void onePlayerScreen()
        {
            UpdateLabelText(Title, "ONE PLAYER GAME");
            UpdateLabelText(P1ScoreLabel, "Player 1 Score - 0");
            UpdateLabelText(P2ScoreLabel, "Player 2 Score - 0");
            SetLabelVisibility(TimeLabel, Visibility.Visible);
            SetLabelVisibility(P1ScoreLabel, Visibility.Visible);
            SetLabelVisibility(P2ScoreLabel, Visibility.Hidden);
            SetLabelVisibility(highScoreTable, Visibility.Hidden);
        }

        private void twoPlayersScreen()
        {
            UpdateLabelText(Title, "TWO PLAYERS GAME");
            UpdateLabelText(P1ScoreLabel, "Player 1 Score - 0");
            UpdateLabelText(P2ScoreLabel, "Player 2 Score - 0");
            SetLabelVisibility(TimeLabel, Visibility.Visible);
            SetLabelVisibility(P1ScoreLabel, Visibility.Visible);
            SetLabelVisibility(P2ScoreLabel, Visibility.Visible);
            SetLabelVisibility(highScoreTable, Visibility.Hidden);
        }

        private void highScoreScreen()
        {
            string highScorePlaylist = playersList[0].PlayerName + "  -  " + playersList[0].Score + Environment.NewLine +
                                        playersList[1].PlayerName + "  -  " + playersList[1].Score + Environment.NewLine +
                                        playersList[2].PlayerName + "  -  " + playersList[2].Score;


            UpdateLabelText(highScoreTable, highScorePlaylist);

            UpdateLabelText(Title, "High Score");
            SetLabelVisibility(TimeLabel, Visibility.Hidden);
            SetLabelVisibility(P1ScoreLabel, Visibility.Hidden);
            SetLabelVisibility(P2ScoreLabel, Visibility.Hidden);
            SetLabelVisibility(highScoreTable, Visibility.Visible);
        }

        void SetHighScore(Player player)
        {
            playersList.Add(player);

            //order the list
            OrderTheList();

            playersList = playersList.GetRange(0, 3);

        }

        void SetLabelVisibility(Label l, Visibility v)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                l.Visibility = v;
            }), DispatcherPriority.Background);
        }
               

        void UpdateLabelText(Label l, string s)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {                
                l.Content = s;
            }), DispatcherPriority.Background);
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

         public static List<Player> playersList = new List<Player>();
        

        private void LoadHighScoreList()
        {
            try
            {
                //load players from file
                string fileName = "players.json";
                string jsonString = File.ReadAllText(fileName);
                playersList = JsonSerializer.Deserialize<List<Player>>(jsonString)!;

                

                
            }
            catch (Exception)
            {
                //load default if player.json not found
                if (!playersList.Any())
                {
                    playersDefault();
                    savePlayersFile();
                }
                string fileName = "players.json";
                string jsonString = File.ReadAllText(fileName);
                playersList = JsonSerializer.Deserialize<List<Player>>(jsonString)!;
            }

            OrderTheList();

            highScoreScreen();

        }

        string GetName(string playernum)
        {

            return Application.Current.Dispatcher.Invoke(() =>
            {
                PlayersNameDialog playersNameDialog = new PlayersNameDialog(playernum);
                //playersNameDialog.PlayerNum = playernum;
                playersNameDialog.ShowDialog();
                playernum = playersNameDialog.Name;


                return playernum;
            });

        }

        void OrderTheList()
        {
            playersList = playersList.OrderByDescending(p => p.Score).ToList();
        }

        void playersDefault()
        {
            Player player1 = new Player();  
            player1.Score = 10;
            player1.PlayerName = "AAA";

            playersList.Add(player1);

            Player player2 = new Player();
            player2.Score = 1;
            player2.PlayerName = "BBB";

            playersList.Add(player2);

            Player player3 = new Player();
            player3.Score = 2;
            player3.PlayerName = "CCC";

            playersList.Add(player3);
        }
        void savePlayersFile()
        {
            string fileName = "players.json";

            string jsonString = JsonSerializer.Serialize(playersList);
            File.WriteAllText(fileName, jsonString);
        }
    }
}


