using GQChat.Other.Pages;
using System.Net.Sockets;

namespace GQChat.Other.Class
{
    public static class Data
    {
        //Прочее

        public const string IpServer = "127.0.0.1";//Ip сервера
        public const int PortServer = 908;//порт сервера

        public class MessageListBox//Для показа сообщений в listbox
        {
            public MessageListBox(string nick, string text)
            {
                Nick = nick;
                Text = text;
            }

            public string Text { get; set; }//Текст сообщения
            public string Nick { get; set; }//Ник
        }

        //Основное

        public static object Pages { get; set; } = new LoginAccount();//Страница
        public static bool NewPages { get; set; } = true;//Есть новая страница
        public static string NameTitle { get; set; } = "GQChat: Вход";
        public static TcpClient TcpClient { get; set; }//Сокет!
        public static bool LoginSucces { get; set; }//Вход выполнен?
    }
}
