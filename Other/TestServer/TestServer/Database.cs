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

            OleDbCommand command = new OleDbCommand($"SELECT Acc_Nick FROM [Accounts] WHERE Acc_Email = ('{email}')", connection);
            string answer = command.ExecuteReader().ToString();
            connection.Close();

            return answer;
        }

        public static void GetClientInfo(string email, string passworld)//TODO
        {

        }

        public static bool CheckClientPassworld(string email, string passworld)//Проверка пароля аккаунта
        {

            OleDbConnection connecton = new OleDbConnection(ConnectCmd);
            connecton.Open();

            int j = 0;
            int InId = 0;
            for (int i = 0; i < 16; i++)
            {
                j += 1;

                // создаем запрос к БД MS Access
                OleDbCommand command = new OleDbCommand($"SELECT Acc_Email FROM [Accounts] WHERE Acc_ID = {j}", connecton);

                if (email != command.ExecuteScalar().ToString())
                {
                    if (j >= 16)
                    {
                        return false; 
                    }
                }
                else
                {
                    InId = j;
                    return true;
                }
            }

            Console.WriteLine(InId);
            OleDbCommand command2 = new OleDbCommand($"SELECT Acc_Password FROM [Accounts] WHERE Acc_ID = {InId}", connecton);
            Console.WriteLine(command2.ExecuteScalar().ToString());//
            if (passworld != command2.ExecuteScalar().ToString())
            {
                return false;
            }

            connecton.Close();
            return true;
        }

        public static bool CheckClientEmail(string email)//Проверка email
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            int j = 0;
            for (int i = 0; i < 16; i++)
            {
                j += 1;

                // создаем запрос к БД MS Access
                OleDbCommand command = new OleDbCommand($"SELECT Acc_Email FROM [Accounts] WHERE Acc_ID = {j}", connection);

                if (email != command.ExecuteScalar().ToString())
                {
                    if (j >= 16)
                    {
                        return false; //P.S. Если он нечего не найдёт, то будет исключение
                    }
                }
                else
                {
                    return true;
                }
            }

            connection.Close();
            return true;
        }

        public static void AccountAdd(string email, string passworld, string nick)//Добавить в аккаунт ERROR
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"INSERT INTO [Accounts] (Acc_Email, Acc_Password, Acc_Nick) VALUES ('{email}', '{passworld}', '{nick}')", connection);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            var set = (Settings)Data.Settings;
            return set.LastId;
        }
    }
}
