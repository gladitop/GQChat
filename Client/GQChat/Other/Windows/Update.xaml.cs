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
using System.Net;
using GQChat.Other.Class;

namespace GQChat.Other.Windows
{
    /// <summary>
    /// Логика взаимодействия для Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
        }

        private async void btCheck_Click(object sender, RoutedEventArgs e)//Проверка и обновление
        {
            using (WebClient web = new WebClient())
            {
                string ver = web.DownloadString(Data.CheckVer);

                if (ver == Data.Ver)
                {
                    MessageBox.Show("У вас последние версия!", Title, MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else
                {
                    string mes = web.DownloadString(Data.URLMessages);
                    MessageBoxResult temp = MessageBox.Show($"Новая версия {ver}\n" +
                        $"Описание: {mes}\n" +
                        $"Обновить?", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (temp == MessageBoxResult.Yes) 
                    {
                        web.DownloadProgressChanged += (s, ee) => 
                        { 
                            progress.Value = ee.ProgressPercentage; 
                        };
                        await web.DownloadFileTaskAsync(Data.DownloadProgram, 
                            $"{Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)}/InstallGQChat.exe");

                        MessageBoxResult temp1 = MessageBox.Show("Файл скачен! Запустить?",
                            Title, MessageBoxButton.YesNo, 
                            MessageBoxImage.Question);

                        if (temp1 == MessageBoxResult.Yes)
                        {
                            //TODO Сделать систему удаление программы
                        }
                        else
                        {
                            MessageBox.Show("ТОГДА ЗАЧЕМ ТЫ ЭТО СКАЧИВАЛ!?", Title, MessageBoxButton.OK,
                                MessageBoxImage.Stop);
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }
    }
}
