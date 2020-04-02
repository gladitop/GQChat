using System.Data.OleDb;

namespace Server
{
    public static class Database///TODO Проверить команды sql
    {
        public const string ConnectCmd = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        public static string GetNickClient(string email)
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT * FROM Accounts WHERE Email = {email}", connection);
            string answer = command.ExecuteReader().ToString();
            connection.Close();

            return answer;
        }

        public static void GetClientInfo(string email, string passworld)//TODO
        {

        }

        public static bool CheckClientPassworld(string passworld)//Проверка пароля аккаунта
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(ConnectCmd);
                connection.Open();

                OleDbCommand command = new OleDbCommand($"SELECT * FROM Accounts WHERE Passworld = {passworld}", connection);
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

                OleDbCommand command = new OleDbCommand($"SELECT * FROM Accounts WHERE Email = {email}", connection);
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

        public static int GetLastIdAccount()
        {
            OleDbConnection connection = new OleDbConnection(ConnectCmd);
            connection.Open();

            OleDbCommand command = new OleDbCommand($"SELECT COUNT(*) FROM Accounts", connection);
            int answer = int.Parse(command.ExecuteReader().ToString());
            connection.Close();

            return ++answer;
        }

        public static void AccountAdd(string email, string passworld, string nick, int id)//Добавить в аккаунт
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
