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
                string passworld, long id, UserAvatar userAvatar, bool offical)
            {
                ClientSocket = tcpClient;
                Nick = nick;
                Email = email;
                Passworld = passworld;
                ID = id;
                UserAvatar = userAvatar;
                Offical = offical;
            }

            public ClientConnectOnly() { }

            public TcpClient ClientSocket { get; set; }//Сокет
            public string Nick { get; set; }//Ник
            public string Email { get; set; }//Почта
            public string Passworld { get; set; }//Пароль
            public long ID { get; set; }//ID клиента
            public UserAvatar UserAvatar { get; set; }//Аватар клиента
            public bool Offical { get; set; }//Аккаунт официальный?
        }

        public class ClientConnectOffline//инфа о клиенте (офлайн) (ТОЛЬКО ДЛЯ СЕРВЕРА)
        {
            public ClientConnectOffline(long id, string nick, string passworld, UserAvatar userAvatar,
                string email, bool offical)
            {
                ID = id;
                Nick = nick;
                Passworld = passworld;
                UserAvatar = userAvatar;
                Email = email;
                Offical = offical;
            }

            public long ID { get; set; }
            public string Nick { get; set; }
            public string Passworld { get; set; }
            public UserAvatar UserAvatar { get; set; }//Аватарка
            public string Email { get; set; }
            public bool Offical { get; set; }//Официальный аккаунт
        }

        public class InfoClientMessInfoChat//Информация для IMessageInfoChat
        {
            public InfoClientMessInfoChat(long id, TypeUserInfoMess typeUser)
            {
                ID = id;
                TypeClient = typeUser;
            }

            public InfoClientMessInfoChat()
            {
                //Опять заглушка
            }

            public long ID { get; set; }
            public TypeUserInfoMess TypeClient { get; set; }//Тип клиента
        }

        public class IMessageInfoChat//Информация для отдельного чата
        {
            public IMessageInfoChat(long lastId, string nameTable, long id,
                long client1, long client2)
            {
                LastID = lastId;
                NameTable = nameTable;
                ID = id;
                ID1 = client1;
                ID2 = client2;
            }

            public IMessageInfoChat(string test)
            {
                NameTable = test;
                //Это нужно чтобы сделать инцилизацию.
                //И смотреть команду %MSE
            }

            public long LastID { get; set; }//id последниго сообщение
            public string NameTable { get; set; }//Имя таблицы в базе данных
            // w_{id1}:{id2}

            public long ID { get; set; }//Id этого чата
            public long ID1 { get; set; }//Id первого клиента
            public long ID2 { get; set; }//Id второго клиента
        }

        //Перечесление

        public enum UserAvatar// Тип аватарки клиента
        {
            Avatar1 = 1,
            Avatar2 = 2,
            Avatar3 = 3,
            Avatar4 = 4,
            Avatar5 = 5,
            Custom = 6 //Своя загружаная аватарка
        }

        public enum TypeUserInfoMess//Тип клиента для сообщение отдельного чата
        {
            Sender = 0,//Отправитель
            Recipient = 1//Получатель
        }
    }
}
