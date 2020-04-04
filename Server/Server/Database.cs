using System;
using System.Data.OleDb;

namespace Server
{
    public static class Database///TODO Проверить команды sql (готово)
    { 
        //Думаю лучше с база данной работать с 64 битной системой, а не 32 битной
        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DataBase.mdb;";

        public static string GetMessageCountLast(long messCount)//TODO
        {


            return "";
        }

        public static long GetIdClient(string email)//Получить id клиента
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT Acc_ID FROM [Accounts] WHERE Acc_Email = {email}",
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return long.Parse(answer);
        }

        public static string GetNickClient(string email)//Получить ник
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT Acc_Nick FROM [Accounts] WHERE Acc_Email = {email}", 
                connection);

            string answer = command.ExecuteScalar().ToString();
            connection.Close();

            return answer;
        }

        public static bool CheckClientPassworld(string email)//Проверка пароля аккаунта
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"SELECT Acc_Password FROM [Accounts] WHERE Acc_Email = {email}",
                    connection);

                if (email == command.ExecuteScalar().ToString())
                {
                    goto linkCheckClientPasswordTrue;
                }
                else
                {
                    goto linkCheckClientPasswordFalse;
                }

            linkCheckClientPasswordTrue:
                connection.Close();
                return true;

            linkCheckClientPasswordFalse:
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

                OleDbCommand command = new OleDbCommand($"SELECT Acc_Email FROM [Accounts] WHERE Acc_Email = {email}",
                    connection);

                if (email == command.ExecuteScalar().ToString())
                {
                    goto linkCheckClientEmailTrue;
                }
                else
                {
                    goto linkCheckClientEmailFalse;
                }

            linkCheckClientEmailTrue:
                connection.Close();
                return true;

            linkCheckClientEmailFalse:
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            //P.S. Если он нечего не найдёт, то будет исключение
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

        public static void AccountAdd(string email, string passworld, string nick)//Добавить в аккаунт ERROR
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"INSERT INTO [Accounts] (Acc_Email, Acc_Password, Acc_Nick)" +
                    $" VALUES ('{email}', '{passworld}', '{nick}')", connection);

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
