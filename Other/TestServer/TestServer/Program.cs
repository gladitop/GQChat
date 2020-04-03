using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Server;

namespace TestServer
{
    class Program
    {
        static TcpListener server = new TcpListener(IPAddress.Any, 908);
        static StreamWriter sw = new StreamWriter("Log.txt");

        static void Main(string[] args)
        {
            Console.Title = "Server";
            WriteLine("Загрузка сервера...", ConsoleColor.Yellow);
            sw.AutoFlush = true;
            server.Start();

            Thread thread = new Thread(new ThreadStart(NewConnect));
            thread.IsBackground = true;
            thread.Start();

            WriteLine("Сервер запущен!", ConsoleColor.Green);
            Console.ReadLine();
            sw.Close();
            Environment.Exit(0);
        }

        static void UpdateMessages(string text, Data.ClientConnectOnly onlyClient)
        {
            try
            {
                WriteLine($"Cообщение в общий чат: {onlyClient.Nick}:{text}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine($"Ошибка в общем чате: {ex.Message}", ConsoleColor.Red);
            }
        }

        static void MessagesClient(object i)
        {
            Data.ClientConnectOnly onlyClient = (Data.ClientConnectOnly)i;
            byte[] buffer = new byte[1024];
            int messi;

            while (true)
            {
                Task.Delay(10).Wait();
            }
        }

        static void CheckNewConnect(object i)
        {
            byte[] buffer = new byte[1024];
            TcpClient client = (TcpClient)i;

            int messi = client.Client.Receive(buffer);

            while (true)
            {
                Task.Delay(10).Wait();
                try
                {

                    linkCommand:

                    messi = client.Client.Receive(buffer);
                    string answer = Encoding.UTF8.GetString(buffer, 0, messi);

                    Console.WriteLine(answer);
                    
                    if (answer.Contains("%REG"))//регистрация
                    {
                        Match regex = Regex.Match(answer, "%REG:(.*):(.*):(.*)");

                        string email = regex.Groups[1].Value;
                        string password = regex.Groups[2].Value;
                        string nick = regex.Groups[3].Value;

                        Database.AddNewAccounts(nick, email, password);

                        client.Client.Send(Encoding.UTF8.GetBytes("%REGOD:Успешная регистрация"));

                        answer = "";
                    }                    
                }
                catch(Exception e)
                {
                    client.Close();
                    WriteLine("Error: Клиент! " + e, ConsoleColor.Red);
                    return;
                }
            }
        }

        static void NewConnect()
        {
            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    Thread thread = new Thread(new ParameterizedThreadStart(CheckNewConnect));
                    thread.Start(client);
                }
                catch { WriteLine("Ошибка клиента!", ConsoleColor.Red); }
            }
        }

        static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            sw.WriteLine($"{DateTime.Now}: {text}");
        }
    }
}
