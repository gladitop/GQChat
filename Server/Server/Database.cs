using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace Server
{
    static public class Database
    {
        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        static public string GetClientInfo(string email, string passworld)
        {


            return "";
        }

        static public bool CheckClientPassworld(string passworld)//Проверка пароля аккаунта
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"SELECT ACC FROM Account WHERE Passworld = {passworld}", connection);
                command.ExecuteReader().ToString();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool CheckClientEmail(string email)//Проверка email
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"SELECT ACC FROM Account WHERE Email = {email}", connection);
                string answer = command.ExecuteReader().ToString();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public void AccountAdd(string email, string passworld)//Добавить в аккаунт
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"INSERT INTO Accounts (Email, Passworld) " +
                $"VALUES ({email}, {passworld})", connection);
            connection.Close();

            command.ExecuteNonQuery();
        }
    }
}
