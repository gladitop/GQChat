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
using System.Threading;
using GQChat.Other.Pages;
using GQChat.Other.Class;

namespace GQChat
{
    /// <summary>
    /// Логика взаимодействия для LoginInAccount.xaml
    /// </summary>
    public partial class LoginInAccount : Window
    {
        Thread thread;

        public LoginInAccount()
        {
            InitializeComponent();
            thread = new Thread(new ThreadStart(Update));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Update()
        {
            while (true)
            {
                Task.Delay(50).Wait();

                this.Dispatcher.Invoke(new Action(() =>
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
                        return;
                    }
                }));
            }
        }
    }
}
