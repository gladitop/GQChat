﻿using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static readonly TcpListener server = new TcpListener(IPAddress.Any, 908);//Порт
        private static readonly StreamWriter sw = new StreamWriter("Log.txt");//Где сохранить логи
        const int delayCheckClient = 10000;//Сколько ждать при проверки клиентов

        //static NetworkStream networkStream;

        private static void Main(string[] args)//Запуск сервера
        {
            Console.Title = "Server";

            WriteLine("Загрузка конфига...", ConsoleColor.Yellow);

            SettingsManager.Load();

            WriteLine("Загрузка сервера...", ConsoleColor.Yellow);

            sw.AutoFlush = true;
            server.Start();

            Thread thread = new Thread(new ThreadStart(NewConnect))
            {
                IsBackground = true
            };
            thread.Start();

            WriteLine("Подключение системы очистки...", ConsoleColor.Yellow);

            thread = new Thread(new ParameterizedThreadStart(CheckConnectClients));
            thread.Start(delayCheckClient);

            WriteLine("Сервер запущен!", ConsoleColor.Green);
            Console.ReadLine();

            WriteLine("Завершение работы...", ConsoleColor.Yellow);

            sw.Close();
            DisconnectClients();
            thread.Abort();
            Environment.Exit(0);
        }

        static void CheckConnectClients(object i)//Проверяет всех клиентов на подключение
        {
            //Загрузка параметров

            int delayCheck = (int)i;

            //Прочее
            IPEndPoint ipClient;//Ip клиента для проверки
            Ping pingClient;//Для ping
            PingReply pingReply;//Результат ping
            bool haveClient = false;//Есть клиенты которые не прошли проверку?
            long CountClient = 0;//Количество клиентов которые не прошли проверку

            while (true)
            {
                Task.Delay(delayCheck).Wait();
                foreach (Data.ClientConnectOnly connectOnly in Data.ClientsOnlyData)
                {
                    ipClient = (IPEndPoint)connectOnly.ClientSocket.Client.RemoteEndPoint;
                    pingClient = new Ping();
                    pingReply = pingClient.Send(ipClient.Address);

                    if (pingReply.Status != IPStatus.Success)
                    {
                        connectOnly.ClientSocket.Close();
                        Data.ClientsOnlyData.Remove(connectOnly);
                        WriteLine($"При проверки обнаружен клиент: {connectOnly.ID}", ConsoleColor.Red);
                        haveClient = true;
                        CountClient++;
                    }
                }

                if (haveClient)
                {
                    WriteLine($"Проверка завершина! Всего клиентов: {CountClient}", ConsoleColor.Green);
                }
            }
        }

        private static void DisconnectClients()//Отключение всеъ клиентов
        {
            foreach (Data.ClientConnectOnly client in Data.ClientsOnlyData)
            {
                client.ClientSocket.Close();
                WriteLine($"Клиент {client.Email}, {client.Passworld} отключился", ConsoleColor.Green);
            }
        }

        private static void UpdateMessages(string text, Data.ClientConnectOnly onlyClient)//Для общего чата
        {
            try
            {
                WriteLine($"Cообщение в общий чат: {onlyClient.Nick}:{text}", ConsoleColor.Green);
                WriteLine("Отправка сообщениие в этот чат...", ConsoleColor.Yellow);

                byte[] answer = new byte[1024];
                answer = Encoding.UTF8.GetBytes(text);

                foreach (Data.ClientConnectOnly client in Data.ClientsOnlyData)
                {
                    client.ClientSocket.Client.Send(answer);
                }

                WriteLine($"Сообщение '{text}', отправлено!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine($"Ошибка в общем чате: {ex.Message}", ConsoleColor.Red);
                WriteLine($"Клиент: {onlyClient.Nick} {onlyClient.Email}", ConsoleColor.Green);
                onlyClient.ClientSocket.Close();
                Data.ClientsOnlyData.Remove(onlyClient);
            }
        }

        public static void MessagesMain(string text)//Отправка сообщения в общий чат
        {
            WriteLine($"Сообщение в общий чат: {text}", ConsoleColor.Green);
            byte[] buffer = new byte[1024];

            //%MME - Получение сообщение в общий чат

            buffer = Encoding.UTF8.GetBytes($"%MME:{text}");

            foreach (Data.ClientConnectOnly client in Data.ClientsOnlyData)
            {
                try
                {
                    client.ClientSocket.Client.Send(buffer);
                }
                catch
                {
                    WriteLine($"Ошибка клиента: {client.Nick}, {client.Email}", ConsoleColor.Red);
                    client.ClientSocket.Close();
                    Data.ClientsOnlyData.Remove(client);
                }
            }
        }

        private static void MessagesClient(object i)//Команды от клиента
        {
            Data.ClientConnectOnly onlyClient = (Data.ClientConnectOnly)i;
            byte[] buffer = new byte[1024];
            int messi;
            string answer;

            while (true)
            {
                Task.Delay(10).Wait();

                try
                {
                    //Пример: %MES:Hello! (Ник мы уже знаем)
                    messi = onlyClient.ClientSocket.Client.Receive(buffer);
                    answer = Encoding.UTF8.GetString(buffer, 0, messi);
                }
                catch (Exception ex)
                {
                    onlyClient.ClientSocket.Close();
                    Data.ClientsOnlyData.Remove(onlyClient);
                    WriteLine($"Клиент вышел: {ex.Message}", ConsoleColor.Red);
                    return;
                }

                if (answer.Contains("%MES"))//Для общего чата
                {
                    Match regex = Regex.Match(answer, "%MES:(.*)");
                    string messagesText = regex.Groups[1].Value;

                    MessagesMain(messagesText);
                }
                else if (answer.Contains("%NCT"))//Новый чат
                {

                }
                else if (answer.Contains("%MSE"))//Для отдельного чата (отправка)
                {

                }
                else if (answer.Contains("%UPM"))//Отправить последнии сообщение (обновление сообщений) TODO
                {
                    try
                    {
                        WriteLine($"Выполнения запроса: '%UPM' от: {onlyClient.Nick}, {onlyClient.ID}",
                            ConsoleColor.Yellow);
                        Match regex = Regex.Match(answer, "%UPM:(.*):(.*)");
                        long idChat = long.Parse(regex.Groups[1].Value);
                        long countMess = long.Parse(regex.Groups[2].Value);

                        for (long Iupm = 0; Iupm < countMess; Iupm++)
                        {
                            //buffer = Encoding.UTF8.GetBytes();
                            //onlyClient.ClientSocket.Client.Send();
                        }

                        WriteLine($"Готово '%UPM' от: {onlyClient.Nick}, {onlyClient.ID}!",
                            ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"Ошибка: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else if (answer.Contains("%EXI"))//Выход (отключение)
                {
                    onlyClient.ClientSocket.Close();
                    Data.ClientsOnlyData.Remove(onlyClient);
                    return;
                }
                else if (answer.Contains("%INF"))//Получить информацию о аккаунте
                {
                    int idClient = int.Parse(answer.Substring(5));//%INF:{}

                    Data.ClientConnectOffline client = Database.GetClientInfo(idClient);

                    //%INF:{email}:{id}:{nick}:{avatar}

                    if (client.UserAvatar != Data.UserAvatar.Custom)
                    {
                        buffer = Encoding.UTF8.GetBytes($"%INF:{client.Email}:{client.ID}:{client.Nick}:" +
                            $"{client.UserAvatar}");
                    }
                    else
                    {
                        //TODO
                    }
                }
                else if (answer.Contains("%DEL"))//Удалить аккаунт
                {
                    //Подтверждение (пароль)

                    
                }
                else if (answer.Contains("%SЕM"))//Отправить файл (Сообщение) (( В общий чат ))
                {

                }
                else if (answer.Contains("%UUS"))//Обновление клиентов (только онлайн)
                {
                    NetworkStream networkStream = onlyClient.ClientSocket.GetStream();
                    string stringTemp = $"%UUS:{Data.ClientsOnlyData.Count}:";

                    foreach (Data.ClientConnectOnly client in Data.ClientsOnlyData)
                    {
                        stringTemp += $"{client.Nick};{client.ID}:";
                    }
                    Console.WriteLine(stringTemp);

                    byte[] answerUUS = Encoding.UTF8.GetBytes(stringTemp);
                    networkStream.Write(answerUUS, 0, answerUUS.Length);
                }
                else//TODO: Это потом для API!
                {
                    //Обработак ошибок
                    //Просто отправить что команды не известна
                    //Да я ОЧЕНЬ ленивый программист
                    //Gladi (384 до н. э.)
                }
            }
        }

        private static void CheckNewConnect(object i)//Проверка нового подключения
        {
            WriteLine("Новое подключение!", ConsoleColor.Green);
            byte[] buffer = new byte[1024];
            TcpClient client = (TcpClient)i;
            //NetworkStream networkClient = client.GetStream();

            //Task.Delay(30).Wait();//Ждём отправки сообщения
            //int messi = networkClient.Read(buffer, 0, buffer.Length);
            int messi = client.Client.Receive(buffer);
            Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, messi));

            Task.Delay(100).Wait();

            try
            {
                //Провека проги
                if (Encoding.UTF8.GetString(buffer, 0, messi) != "TCPCHAT 1.0")//!!!
                {
                    WriteLine("Ошибка: Cтарый или другой клиент!", ConsoleColor.Red);
                    client.Close();
                    return;//Проверить! (Готово)
                }
            }
            catch
            {
                WriteLine("Ошибка! Клиент!", ConsoleColor.Red);
                client.Close();
                return;
            }


            //Команды


            Task.Delay(10).Wait();


        linkCommand:
            Task.Delay(100).Wait();

            //messi = client.Receive(buffer);
            messi = client.Client.Receive(buffer);
            string answer = Encoding.UTF8.GetString(buffer, 0, messi);
            WriteLine("Проверка нового подклюение...", ConsoleColor.Yellow);

            if (answer.Contains("%REG"))//регистрация
            {
                //email

                Match regex = Regex.Match(answer, "%REG:(.*):(.*):(.*)");//Антон!
                string email = regex.Groups[1].Value;
                Console.WriteLine(@"%REG:(.*):(.*):(.*)");
                Console.WriteLine(answer);
                Console.WriteLine(email);

                //string email = answer.
                //TODO: Сделать проверку email через подтверждение (Нужен smtp сервер)

                //пароль

                string passworld = regex.Groups[2].Value;//TODO: Нужен md5
                Console.WriteLine(passworld);

                //Nick

                string nick = regex.Groups[3].Value;
                Console.WriteLine(nick);

                //Проверка

                bool checkNewAccount = Database.CheckClientEmail(email);
                //Если true, то email такой есть

                if (checkNewAccount)//!!!
                {
                    Console.WriteLine("0");
                    client.Client.Send(Encoding.UTF8.GetBytes("%REGWRONGEMAIL"));
                    goto linkCommand;
                }
                else
                {
                    Console.WriteLine("1");
                    Settings set = (Settings)Data.Settings;
                    set.LastId = Database.GetLastIdAccount() + 1;
                    Database.AccountAdd(email, passworld, nick, set.LastId);
                    client.Client.Send(Encoding.UTF8.GetBytes("%REGOD"));

                    WriteLine($"Новый аккаунт! {email}, {passworld}", ConsoleColor.Green);
                    Data.Settings = set;
                    SettingsManager.Save();
                    return;
                }
            }
            else if (answer.Contains("%LOG"))//Вход
            {
                //email

                answer = Encoding.UTF8.GetString(buffer, 0, messi);

                Match regex = Regex.Match(answer, "%LOG:(.*):(.*)");//Антон!
                string email = regex.Groups[1].Value;
                Console.WriteLine(email);
                //пароль

                string passworld = regex.Groups[2].Value;
                Console.WriteLine(passworld);
                //Проверка email

                bool checkClient = Database.CheckClientEmail(email);

                if (!checkClient)
                {
                    //networkClient.Write(Encoding.UTF8.GetBytes("0"), 0, buffer.Length);
                    client.Client.Send(Encoding.UTF8.GetBytes("%LOGWRONGEMAIL"));// False
                    WriteLine("Неправильный email!", ConsoleColor.Red);
                    goto linkCommand;
                }
                else
                {
                    //Проверка пароля

                    bool checkPassworld = Database.CheckClientPassworld(passworld);

                    if (!checkPassworld)//!!!
                    {
                        Console.WriteLine("%LOGWRONGEPASS");
                        client.Client.Send(Encoding.UTF8.GetBytes("%LOGWRONGEPASS"));
                        WriteLine($"Неправильный пароли в {email}!", ConsoleColor.Red);
                        goto linkCommand;
                    }
                    else
                    {
                        Console.WriteLine("%LOGOD");
                        client.Client.Send(Encoding.UTF8.GetBytes("%LOGOD"));

                        //Инцилизация!

                        Data.ClientConnectOnly onlyClient = new Data.ClientConnectOnly(client,
                            Database.GetNickClient(email), email, passworld, Database.GetIdClient(email), 
                            Data.UserAvatar.Avatar1);

                        Data.ClientsOnlyData.Add(onlyClient);

                        Console.WriteLine($"{onlyClient.Email} {onlyClient.ID} {onlyClient.Nick} " +
                            $"{onlyClient.Passworld}");

                        Thread thread = new Thread(new ParameterizedThreadStart(MessagesClient))
                        {
                            IsBackground = true
                        };

                        thread.Start(onlyClient);
                        WriteLine($"Вход аккаунт! {email}, {passworld}", ConsoleColor.Green);

                        return;
                    }
                }
            }

        }

        private static void NewConnect()//Подключение нового подключения
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

        private static void WriteLine(string text, ConsoleColor color)//Кравивый текст и логи
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            sw.WriteLine($"{DateTime.Now}: {text}");
        }
    }
}
