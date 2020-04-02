using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace Server
{
    static public class Database///TODO Проверить команды sql
    {
        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        static public Data.ClientConnectOnly GetClientInfo(string email, string passworld)//TODO
        {
            return new Data.ClientConnectOnly(new System.Net.Sockets.TcpClient(), "", "");
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
                command.ExecuteReader().ToString();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public int GetLastIdAccount()
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT COUNT(*) FROM Account", connection);
            int answer = int.Parse(command.ExecuteReader().ToString());
            connection.Close();

            return ++answer;
        }

        static public void AccountAdd(string email, string passworld, string nick, int id)//Добавить в аккаунт
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"INSERT INTO Accounts (ID, Email, Passworld, Nick) " +
                $"VALUES ({id}, {email}, {passworld}, {nick})", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
