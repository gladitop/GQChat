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

        public static bool CheckClientPassworld(string passworld)//Проверка пароля аккаунта с помощью data
        {
            try
            {
                OleDbConnection connecton = new OleDbConnection(ConnectCmd);
                connecton.Open();

                // создаем запрос к БД MS Access
                OleDbCommand command = new OleDbCommand($"SELECT Acc_Password FROM [Accounts] WHERE Acc_ID = {Data.InID}", connecton);

                if (passworld != command.ExecuteScalar().ToString())
                {
                    return false;
                }

                connecton.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.ToString());
                return false;
            }
        }

        public static bool CheckClientEmail(string email)//Проверка email
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            long j = 0;
            for (long i = 0; i <= 16; i++)
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
                    Data.InID = j;
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

        /*public static long GetLastIdAccount()//Error! (Исправлено)
        {
            
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT LAST_INSERT_ID(Accounts)", connection);
            long answer = long.Parse(command.ExecuteReader().ToString());
            connection.Close();

            return answer;
            

            var set = (Settings)Data.Settings;
            return set.LastId;
        }*/
        /*public static void GetClientInfo(string email, string passworld)//TODO
        {

        }*/
    }
}
