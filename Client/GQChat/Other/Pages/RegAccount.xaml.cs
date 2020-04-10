using GQChat.Other.Class;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GQChat.Other.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegAccount.xaml
    /// </summary>
    public partial class RegAccount : Page
    {
        public RegAccount()
        {
            InitializeComponent();
        }

        private void btReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Напишите email!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(tbPassworld.Password))
            {
                MessageBox.Show("Напишите пароль!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(tbNick.Text))
            {
                MessageBox.Show("Напишите ник!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tbEmail.Text.Length >= 100)
            {
                MessageBox.Show("Слишком длинный email!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tbPassworld.Password.Length >= 40)
            {
                MessageBox.Show("Слишком длинный пароль!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tbNick.Text.Length >= 20)
            {
                MessageBox.Show("Слишком длинный ник!", "GQChat: Вход", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                using (TcpClient client = new TcpClient(Data.IpServer, Data.PortServer))
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        client.Client.Send(Encoding.UTF8.GetBytes("TCPCHAT 1.0"));

                        Task.Delay(100).Wait();
                        if (!client.Connected)
                        {
                            client.Close();
                            MessageBox.Show("Ошибка при подключение", Data.NameTitle, MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        Task.Delay(100).Wait();

                        client.Client.Send(Encoding.UTF8.GetBytes($"%REG:{tbEmail.Text}:{tbPassworld.Password}:" +
                            $"{tbNick.Text}"));
                        int messi = client.Client.Receive(buffer);

                        string answer = Encoding.UTF8.GetString(buffer, 0, messi);
                        if (answer == "%REGOD")
                        {
                            Data.TcpClient = client;
                            Data.LoginSucces = true;
                        }//TODO Сделать проверку ошибок!
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        MessageBox.Show($"Описание ошибки: {ex.Message}", Data.NameTitle, MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
