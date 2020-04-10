using GQChat.Other.Class;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace GQChat.Other.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginAccount.xaml
    /// </summary>
    public partial class LoginAccount : Page
    {
        public LoginAccount()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Напишите email!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(tbPassworld.Password))
            {
                MessageBox.Show("Напишите пароль!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tbEmail.Text.Length >= 100)
            {
                MessageBox.Show("Слишком длинный email!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tbPassworld.Password.Length >= 40)
            {
                MessageBox.Show("Слишком длинный пароль!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                /*
                using (TcpClient client = new TcpClient(Data.IpServer, Data.PortServer))
                {
                    byte[] buffer = new byte[1024];
                    client.Client.Send(Encoding.UTF8.GetBytes("TCPCHAT 1.0"));
                    Task.Delay(100).Wait();

                    client.Client.Send(Encoding.UTF8.GetBytes($"%LOG:{tbEmail.Text}:{tbPassworld.Password}"));
                    Task.Delay(100).Wait();

                    int messi = client.Client.Receive(buffer);

                    string answer = Encoding.UTF8.GetString(buffer, 0, messi);
                    if (answer == "%LOGOD")
                    {
                        Data.TcpClient = client;
                        Data.LoginSucces = true;
                    }//TODO Сделать проверку ошибок!
                }
                */

                Data.TcpClient = new TcpClient();
                Data.LoginSucces = true;
            }
        }

        private void btReg_Click(object sender, RoutedEventArgs e)
        {
            Data.Pages = new RegAccount();
            Data.NewPages = true;
        }
    }
}
