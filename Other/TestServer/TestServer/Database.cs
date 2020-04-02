using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace TestServer
{
    class Database
    {

        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DataBase.mdb;";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DataBase.mdb;";

        public static void AddNewAccounts(string nick, string email, string password)
        {
            OleDbConnection myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            //ToDo("Когда нибудь, когда сакура расцветёт в ночи отражённых лучей луны, мы перестанем нести это бремя идиотского database");

            string query = "INSERT INTO [Accounts] (Acc_Nick, Acc_Email, Acc_Password) VALUES ('Михаил', 'Водитель', 'Привет')";
            //INSERT INTO конечный_объект [(поле1[, поле2[, …]])] VALUES (значение1[, значение2[, …])
            OleDbCommand command = new OleDbCommand(query, myConnection);
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void OleDbCommand()
        {
            //string query = "SELECT Nick From Accounts WHERE ID = 1";
            //comands
            //text.text = command.ExecuteScalar().ToString();

            //OleDbDataReader reader = command.ExecuteReader();
            //while(reader.Read())
            //{
            //    reader[0].ToString();
            //}

            //string query2 = "INSERT INTO Accounts (Nick, Email, Password) VALUES ('nick', 'email', 'password')";
            //OleDbCommand command2 = new OleDbCommand(query, myConnection);
            //command2.ExecuteNonQuery();

            //string query2 = "UPDATE Accounts SET Email = email@gogla.com ID = 2";
            //OleDbCommand command2 = new OleDbCommand(query, myConnection);
            //command2.ExecuteNonQuery();

            //string query2 = "DELETED FROM Accounts ID < 2";
            //OleDbCommand command2 = new OleDbCommand(query, myConnection);
            //command2.ExecuteNonQuery();

            //string query = "SELECT MAX(ID) FROM Accounts"
        }
    }
}
