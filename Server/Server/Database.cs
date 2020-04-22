using System;
using System.ComponentModel;
using System.Data.OleDb;

namespace Server
{
    public static class Database///TODO Проверить команды sql (готово)
    {
        /*
        Думаю лучше с база данной работать с 64 битной системой, а не 32 битной
        (наверно это не правда)
        P.S. Так и есть
        */

        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=GladiData.MDB;";

        public static void CreateNewDialog(long idClient, long idClient2)//Создание диалога
        {
            var setting = (Settings)Data.Settings;
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"CREATE TABLE w_{idClient}_{idClient2} (w_message STRING" +
                $" w_nick STRING)"
                , connection);

            setting.MessageInfoChats.Add(new Data.IMessageInfoChat(0, $"w_{idClient}_{idClient2}"));
            Data.Settings = setting;
            
            SettingsManager.Save();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static Data.ClientConnectOffline GetClientInfo(long id)//Получить инфо о клиенте
        {
            //Ник

            string nick = GetNickClient(id);

            //Пароль

            string passworld = GetClientPassworld(id);

            //Аватарку

            Data.UserAvatar avatar = GetClientAvatar(id);

            //Email

            string email = GetClientEmail(id);

            return new Data.ClientConnectOffline(id, nick, passworld, avatar, email);

            //return new Data.ClientConnectOffline(id, "Gladi", "324252", Data.UserAvatar.Avatar1,
            //    "gladi@gmail.com");
        }

        public static string GetClientEmail(long id)
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT w_email FROM w_accounts WHERE w_id = {id}",//Проверить!
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return answer;
        }

        public static Data.UserAvatar GetClientAvatar(long id)//Получить аватарку
        {
            //TODO: Добавить в базы данных это надо!

            return Data.UserAvatar.Avatar1;
        }

        public static string GetClientPassworld(long id)//Получить пароль
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT w_pasworld FROM w_accounts WHERE w_id = {id}",//Проверить!
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return answer;
        }

        public static long GetMessageCountLast(long messCount)//Получить последние id сообщения в общем чате
        {
            Settings settings = (Settings)Data.Settings;

            return settings.LastIdMessMain;
        }

        public static long GetIdClient(string email)//Получить id клиента
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT w_id FROM w_accounts WHERE w_email = '{email}'",
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return long.Parse(answer);
        }

        public static string GetNickClient(string email)//Получить ник (email)
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT w_nick FROM w_accounts WHERE w_email = '{email}'",
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return answer;
        }

        public static string GetNickClient(long id)//Получить ник (id)
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT w_nick FROM w_accounts WHERE w_id = {id}",//Проверить!
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return answer;
        }

        public static bool CheckClientPassworld(string passworld)//Проверка пароля аккаунта
        {
            try
            {
                // true - правильный пароль
                // false - неверный пароль

                //SELECT COUNT(*) FROM People Where Имя = 'Mihail'

                //OleDbCommand command = new OleDbCommand($"INSERT INTO w_accounts (w_id, w_email, w_passworld, w_nick)" +
                //    $" VALUES ({id}, '{email}', '{passworld}', '{nick}')", connection);

                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                OleDbCommand command = new OleDbCommand($"SELECT COUNT(*) FROM w_accounts Where w_passworld = '{passworld}'",
                    connection);

                connection.Open();
                int answer = (int)command.ExecuteScalar();//!!!

                if (answer == 0)
                {
                    return false;
                }
                else//И да если тут может быть и два. Это просто показывает количество
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в пароль: {ex.Message}");
                return false;
            }
        }

        public static bool CheckClientEmail(string email)//Проверка email
        {
            // true - есть email
            // false - нет email

            //SELECT COUNT(*) FROM People Where Имя = 'Mihail'

            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            OleDbCommand command = new OleDbCommand($"SELECT COUNT(*) FROM w_accounts Where w_email = '{email}'",
                connection);

            connection.Open();
            int answer = (int)command.ExecuteScalar();//!!!

            if (answer == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static long GetLastIdAccount()//Error! (Исправлено)
        {
            /*
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT LAST_INSERT_ID(Accounts)", connection);
            long answer = long.Parse(command.ExecuteReader().ToString());
            connection.Close();

            return answer;
            */

            Settings set = (Settings)Data.Settings;
            return set.LastId;
        }

        public static void AccountAdd(string email, string passworld, string nick, long id, int avatar, bool offical)//Добавить в аккаунт ERROR
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"INSERT INTO w_accounts (w_id, w_email, w_passworld, w_nick, w_avatar, w_offical)" +
                    $" VALUES ({id}, '{email}', '{passworld}', '{nick}', {avatar}, {offical})", connection);//Новые параметры!
                //-1 это true, а 0 это false

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Добавить аккаунт");
            }
        }
    }
}
