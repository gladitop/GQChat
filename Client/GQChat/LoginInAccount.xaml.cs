using GQChat.Other.Class;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GQChat
{
    /// <summary>
    /// Логика взаимодействия для LoginInAccount.xaml
    /// </summary>
    public partial class LoginInAccount : Window
    {
        private readonly Thread thread;

        public LoginInAccount()
        {
            InitializeComponent();
            thread = new Thread(new ThreadStart(Update))
            {
                IsBackground = true
            };
            thread.Start();
        }

        public void Update()
        {
            while (true)
            {
                Task.Delay(50).Wait();

                Dispatcher.Invoke(new Action(() =>
                {
                    if (Data.NewPages)
                    {
                        Title = Data.NameTitle;
                        frame.Navigate(Data.Pages);
                        Data.NewPages = false;
                    }
                    else if (Data.LoginSucces)
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                        Hide();
                        thread.Abort();
                        return;
                    }
                }));
            }
        }

        private void logininaccount_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
