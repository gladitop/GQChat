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
        //static NetworkStream networkStream;

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

        static void MessagesClient(object i)
        { 

            while (true)
            {
                Task.Delay(10).Wait();


            }
        }

        static void CheckNewConnect(object i)
        {
            byte[] buffer = new byte[1024];
            TcpClient client = (TcpClient)i;
            //NetworkStream networkClient = client.GetStream();

            //Task.Delay(30).Wait();//Ждём отправки сообщения
            //int messi = networkClient.Read(buffer, 0, buffer.Length);
            int messi = client.Client.Receive(buffer);

            try
            {
                //Провека проги
                if (Encoding.UTF8.GetString(buffer, 0, messi) != "TCPCHAT 1.0")
                {
                    WriteLine("Ошибка: Cтарый или другой клиент!", ConsoleColor.Red);
                    client.Close();
                    return;//Проверить!
                }
            }
            catch
            {
                WriteLine("Ошибка! Клиент!", ConsoleColor.Red);
                client.Close();
                return;
            }

            //Команды

            while (true)
            {
                Task.Delay(10).Wait();
                try
                {

                linkCommand:

                    //messi = client.Receive(buffer);
                    messi = client.Client.Receive(buffer);

                    if (Encoding.UTF8.GetString(buffer, 0, messi) == "REG")//регистрация
                    {
                        //email

                        messi = client.Client.Receive(buffer);
                        string email = Encoding.UTF8.GetString(buffer, 0, messi);
                        //TODO: Сделать проверку email через подтверждение (Нужен smtp сервер)

                        //пароль

                        messi = client.Client.Receive(buffer);
                        string passworld = Encoding.UTF8.GetString(buffer, 0, messi);//TODO: Нужен md5

                        //Nick

                        messi = client.Client.Receive(buffer);
                        string nick = Encoding.UTF8.GetString(buffer, 0, messi);

                        //Проверка

                        bool checkNewAccount = Database.CheckClientEmail(email);

                        if (checkNewAccount)
                        {
                            client.Client.Send(Encoding.UTF8.GetBytes("0"));
                            goto linkCommand;
                        }
                        else
                        {
                            Database.AccountAdd(email, passworld, nick, Database.GetLastIdAccount());
                            client.Client.Send(Encoding.UTF8.GetBytes("1"));

                            WriteLine($"Новый аккаунт! {email}, {passworld}", ConsoleColor.Green);
                            return;
                        }
                    }
                    else if (Encoding.UTF8.GetString(buffer, 0, messi) == "LOG")//Вход
                    {
                        //email

                        messi = client.Client.Receive(buffer);
                        string email = Encoding.UTF8.GetString(buffer, 0, messi);

                        //пароль

                        messi = client.Client.Receive(buffer);
                        string passworld = Encoding.UTF8.GetString(buffer, 0, messi);

                        //Проверка email

                        bool checkClient = Database.CheckClientEmail(email);

                        if (!checkClient)
                        {
                            //networkClient.Write(Encoding.UTF8.GetBytes("0"), 0, buffer.Length);
                            client.Client.Send(Encoding.UTF8.GetBytes("0"));// False
                            goto linkCommand;
                        }
                        else
                        {
                            //Проверка пароля

                            client.Client.Send(Encoding.UTF8.GetBytes("1"));// True
                            bool checkPassworld = Database.CheckClientPassworld(passworld);

                            if (!checkPassworld)
                            {
                                client.Client.Send(Encoding.UTF8.GetBytes("0"));
                                goto linkCommand;
                            }
                            else
                            {
                                client.Client.Send(Encoding.UTF8.GetBytes("1"));

                                //Инцилизация!

                                Data.ClientConnectOnly onlyClient = new Data.ClientConnectOnly(client, 
                                    email, passworld);
                                Data.ClientsOnlyData.Add(onlyClient);

                                Thread thread = new Thread(new ParameterizedThreadStart(MessagesClient));
                                thread.IsBackground = true;
                                thread.Start(onlyClient);
                                WriteLine($"Вход аккаунт! {email}, {passworld}", ConsoleColor.Green);

                                return;
                            }
                        }
                    }
                }
                catch
                {
                    client.Close();
                    WriteLine("Error: Клиент!", ConsoleColor.Red);
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
