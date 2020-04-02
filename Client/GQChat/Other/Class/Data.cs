using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GQChat.Other.Pages;
using System.Net.Sockets;

namespace GQChat.Other.Class
{
    static public class Data
    {
        //Прочее

        public const string IpServer = "127.0.0.1";//Ip сервера
        public const int PortServer = 908;//порт сервера

        //Основное

        static public object Pages { get; set; } = new LoginAccount();//Страница
        static public bool NewPages { get; set; } = true;//Есть новая страница
        static public string NameTitle { get; set; } = "GQChat: Вход";
        static public TcpClient TcpClient { get; set; }//Сокет!
        static public bool LoginSucces { get; set; }//Вход выполнин?
    }
}
