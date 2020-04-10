using System.Collections.Generic;
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
                string passworld, long id, UserAvatar userAvatar)
            {
                ClientSocket = tcpClient;
                Nick = nick;
                Email = email;
                Passworld = passworld;
                ID = id;
                UserAvatar = userAvatar;
            }

            public TcpClient ClientSocket { get; set; }//Сокет
            public string Nick { get; set; }//Ник
            public string Email { get; set; }//Почта
            public string Passworld { get; set; }//Пароль
            public long ID { get; set; }//ID клиента
            public UserAvatar UserAvatar { get; set; }//Аватар клиента
        }

        public class ClientConnectOffline//инфа о клиенте (офлайн) (ТОЛЬКО ДЛЯ СЕРВЕРА)
        {
            public ClientConnectOffline(long id, string nick, string passworld, UserAvatar userAvatar,
                string email)
            {
                ID = id;
                Nick = nick;
                Passworld = passworld;
                UserAvatar = userAvatar;
                Email = email;
            }

            public long ID { get; set; }
            public string Nick { get; set; }
            public string Passworld { get; set; }
            public UserAvatar UserAvatar { get; set; }
            public string Email { get; set; }
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

        public enum UserAvatar// Тип аватарки клиента
        {
            Avatar1 = 1,
            Avatar2 = 2,
            Avatar3 = 3,
            Avatar4 = 4,
            Avatar5 = 5,
            Custom = 6 //Своя загружаная аватарка
        }
    }
}
