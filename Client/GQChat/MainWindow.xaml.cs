using GQChat.Other.Class;
using System;
using System.Windows;

namespace GQChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            listMessages.Items.Add(new Data.MessageListBox("TestUser", "Test message"));
            listMessages.Items.Add(new Data.MessageListBox("TestUser1", "Test message1"));
            listMessages.Items.Add(new Data.MessageListBox("TestUser2", "Test messag2"));
            listMessages.Items.Add(new Data.MessageListBox("TestUser3", "Test message3"));
            listMessages.Items.Add(new Data.MessageListBox("TestUser4", "Test message4"));
            listMessages.Items.Add(new Data.MessageListBox("TestUser5", "Test message5"));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
