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

namespace Server
{
    class Program
    {
        static TcpListener server = new TcpListener(IPAddress.Any, 908);
        static StreamWriter sw = new StreamWriter("Log.txt");
        static NetworkStream networkStream;

        static void Main(string[] args)
        {
            Console.Title = "Server";
            WriteLine("Загрузка сервера...", ConsoleColor.Yellow);
            sw.AutoFlush = true;
            server.Start();
            networkStream = new NetworkStream(server.Server);

            Thread thread = new Thread(new ThreadStart(NewConnect));
            thread.IsBackground = true;
            thread.Start();

            Console.ReadLine();
            sw.Close();
            Environment.Exit(0);
        }

        static void CheckNewConnect(object i)
        {
            byte[] buffer = new byte[1024];
            TcpClient client = (TcpClient)i;
            NetworkStream networkClient = client.GetStream();
            //TODO

            int messi = client.(buffer);

            //Провека проги
            if (Encoding.UTF8.GetString(buffer, 0, messi) != "TCPCHAT 1.0")
            {
                WriteLine("Ошибка: Cтарый или другой клиент!", ConsoleColor.Red);
                client.Close();
                return;//Проверить!
            }

            //Команды

            while (true)
            {
                Task.Delay(10).Wait();
                
                messi = client.Receive(buffer);

                if (Encoding.UTF8.GetString(buffer) == "REG")//регистрация
                {
                    //email

                    //пароль
                }
                else if (Encoding.UTF8.GetString(buffer) == "LOG")//Вход
                {

                }
                else if (Encoding.UTF8.GetString(buffer) == "MES")//Сообщение
                {

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
