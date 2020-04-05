using System;
using System.Data.OleDb;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Server
{
    public static class Database///TODO Проверить команды sql (готово)
    { 
        /*
        Думаю лучше с база данной работать с 64 битной системой, а не 32 битной
        (наверно это не правда)
        */
       
        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=GladiData.MDB;";

        public static string GetMessageCountLast(long messCount)//TODO
        {


            return "";
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

        public static string GetNickClient(string email)//Получить ник
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT w_nick FROM w_accounts WHERE w_email = '{email}'", 
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

                OleDbCommand command = new OleDbCommand($"SELECT w_passworld FROM w_accounts WHERE w_email = '{email}'",
                    connection);

                string emailCheck = command.ExecuteScalar().ToString();

                if (emailCheck == null)
                    goto linkCheckClientPasswordFalse;

                if (email == emailCheck)
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
                Console.WriteLine(ex.Message + " Проверка passworld");
                return false;
            }

            //P.S. Если он нечего не найдёт, то будет исключение
        }

        public static bool CheckClientEmail(string email)//Проверка email
        {
            /*
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                //OleDbCommand command = new OleDbCommand($"SELECT w_email FROM w_accounts WHERE w_email = '{email}'",
                //    connection);

                OleDbCommand command = new OleDbCommand($"SELECT EXISTS (SELECT w_email FROM w_accounts WHERE w_email = '{email}' = 1)", connection);

                string lol = string.Empty == command.ExecuteScalar().ToString() ? 
                    "" : command.ExecuteScalar().ToString();
                Console.WriteLine(lol = " LOL");

                if (Convert.ToString(command.ExecuteScalar()).ToString() == "")
                    goto linkCheckClientEmailFalse;

                string emailCheck = command.ExecuteScalar().ToString();

                emailCheck = Convert.ToString(emailCheck);
                Console.WriteLine(emailCheck + " Это проверка email");
                
                //read.Close();

                //emailCheck = command.ExecuteScalar().ToString();

                if (email == emailCheck)
                {
                    Console.WriteLine("true");
                    goto linkCheckClientEmailTrue;//Есть email
                }
                else if (email != emailCheck)
                {
                    Console.WriteLine("False");
                    goto linkCheckClientEmailFalse;//Нет email!
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
                Console.WriteLine(ex.Message + " Проверка email");
                return false;
            }

            //P.S. Если он нечего не найдёт, то будет исключение

            Эти сатанисты не сделали нормальные sql команды!!!
            */

            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            //true - Есть email!
            //false - Нет email!

            var settings = (Settings)Data.Settings;//Антон!
            long j = 0;
            for (long i = 0; i <= settings.LastId; i++)
            {
                j += 1;

                // создаем запрос к БД MS Access
                OleDbCommand command = new OleDbCommand($"SELECT w_email FROM w_accounts WHERE w_id = {j}", connection);
                //SELECT w_email FROM w_accounts WHERE w_email = '{email}'
                OleDbDataReader reader = command.ExecuteReader();

                //reader.FieldCount; TODO

                while (reader.Read())
                {
                    string lol = "";
                    for (int ii = 0; ii <= reader.FieldCount; ii++)//!!! Нет long
                    {
                        try
                        {
                            lol = Convert.ToString(reader[ii]);
                            Console.WriteLine(lol);

                            if(lol == email)
                                return true;
                        }
                        catch { Console.WriteLine($"Ошибка в {ii} и {lol}"); }
                    }
                }

                /*
                if (!string.IsNullOrWhiteSpace(command.ExecuteScalar().ToString()))
                {
                    if (email == command.ExecuteScalar().ToString())//Ссылка на объект не указывает на экземпляр объекта!
                    {
                        return true;
                    }
                }
                */

                reader.Close();
            }

            connection.Close();
            return false;
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

        public static void AccountAdd(string email, string passworld, string nick, long id)//Добавить в аккаунт ERROR
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"INSERT INTO w_accounts (w_id, w_email, w_passworld, w_nick)" +
                    $" VALUES ({id}, '{email}', '{passworld}', '{nick}')", connection);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + " Добавить аккаунт");
            }
        }
    }
}
