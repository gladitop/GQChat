using System;
using System.Data.OleDb;

namespace Server
{
    public static class Database///TODO Проверить команды sql (готово)
    {
        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        public static string GetNickClient(string email)
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT Nick FROM Accounts WHERE Email = {email}", connection);
            string answer = command.ExecuteReader().ToString();
            connection.Close();

            return answer;
        }

        public static void GetClientInfo(string email, string passworld)//TODO
        {

        }

        public static bool CheckClientPassworld(string email)//Проверка пароля аккаунта
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"SELECT Password FROM Accounts WHERE Email = {email}", connection);
                command.ExecuteReader().ToString();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }

            //P.S. Если он нечего не найдёт, то будет исключение
        }

        public static bool CheckClientEmail(string email)//Проверка email
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"SELECT UserId FROM Accounts WHERE Email = {email}", connection);
                command.ExecuteReader().ToString();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return false;
            }

            //P.S. Если он нечего не найдёт, то будет исключение
        }

        public static long GetLastIdAccount()//Error!
        {
            /*
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT LAST_INSERT_ID(Accounts)", connection);
            long answer = long.Parse(command.ExecuteReader().ToString());
            connection.Close();

            return answer;
            */

            var set = (Settings)Data.Settings;
            return set.LastId;
        }

        public static void AccountAdd(string email, string passworld, string nick, long id)//Добавить в аккаунт
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"INSERT INTO [Accounts] (UserId, Email, Password, Nick) VALUES ({id}, '{email}', '{passworld}', '{nick}')", connection);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
