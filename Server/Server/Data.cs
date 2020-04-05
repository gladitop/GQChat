using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Server
{
    public static class Data
    {
        //Все подключенные клиенты
        public static List<ClientConnectOnly> ClientsOnlyData { get; set; } = new List<ClientConnectOnly>();
        public static object Settings { get; set; }//Настройки
        public static long InID { get; set; }//специально для Антона (смотреть класс Database)

        //Прочее

        public class ClientConnectOnly//инфа о клиенте (онлайн) (ТОЛЬКО ДЛЯ СЕРВЕРА)
        {
            public ClientConnectOnly(TcpClient tcpClient, string nick, string email,
                string passworld, long id)
            {
                ClientSocket = tcpClient;
                Nick = nick;
                Email = email;
                Passworld = passworld;
                ID = id;
            }

            public TcpClient ClientSocket { get; set; }//Сокет
            public string Nick { get; set; }//Ник
            public string Email { get; set; }//Почта
            public string Passworld { get; set; }//Пароль
            public long ID { get; set; }//ID клиента
        }

        public class IMessageInfoChat//Информация для отдельного чата
        {
            public IMessageInfoChat(long lastId, string nameTable)
            {
                LastID = lastId;
                NameTable = nameTable;
            }

            public long LastID { get; set; }//id последниго сообщение
            public string NameTable { get; set; }//Имя таблицы в базе данных
        }
    }
}
