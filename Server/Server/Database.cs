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

        static public bool CheckClient(int id)
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT ACC FROM Accounts WHERE ID = {id}", connection);
            string answer = command.ExecuteReader().ToString();
            connection.Close();

            if (string.IsNullOrWhiteSpace(answer))
                return false;
            else
                return true;
        }

        static public void AccountAdd(string email, string passworld)
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
