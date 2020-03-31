using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    static public class Data
    {
        //Прочее

        public struct ClientConnectOnly//инфа о клиенте (онлайн)
        {
            public Socket ClientSocket;
            public int ID;
        }

        /*
        public struct ClientConnectOffline//инфа о клиенте (офнлайн)
        {
            public int ID;
            public string Email;
            public string Passworld;
        }
        

        public struct GeneralChatMess//Сообщение для клиента (Общий чат)
        {
            public int ID;
            public string Text;
        }
        */
    }
}
