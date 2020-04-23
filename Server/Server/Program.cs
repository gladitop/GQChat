using ConsoleTables;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.DesignerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        #region Настройки
        private static readonly TcpListener server = new TcpListener(IPAddress.Any, 908);//Порт
        private static readonly StreamWriter sw = new StreamWriter("Log.txt");//Где сохранить логи
        private const int delayCheckClient = 10000;//Сколько ждать при проверки клиентов
        private const string ver = "1.0";//Версия сервера
        #endregion

        #region Переменные
        private static string answerCommand;//Для обработак команд
        //static NetworkStream networkStream;
        #endregion

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

            WriteLine("Создание папок...", ConsoleColor.Yellow);

            if (!Directory.Exists("Avatars"))
            {
                Directory.CreateDirectory("Avatars");
                WriteLine("Создана папка Avatars", ConsoleColor.Green);
            }

            if (!Directory.Exists("Files"))
            {
                Directory.CreateDirectory("Files");
                WriteLine("Создана папка Files", ConsoleColor.Green);
            }

            WriteLine("Сервер запущен!", ConsoleColor.Green);

            //Команды

            while (true)
            {
                answerCommand = Console.ReadLine();

                if (answerCommand.ToLower() == "exit")//Отключение сервера
                {
                    OffServer();

                    //Ожидание отключение
                    while (true)
                    {
                        Task.Delay(1000).Wait();
                    }
                }
                else if (answerCommand.ToLower() == "client c")//Количество клиентов (в онлайн)
                {
                    ShowCountClient();
                }
                else if (answerCommand.ToLower() == "client f")//Показать количество клиентов (в таблице)
                {
                    ShowCountClientTable();
                }
                else if (answerCommand.ToLower() == "client l")//Показать всех клиентов из базы данных
                {
                    ShowAllClient();
                }
                else if (answerCommand.ToLower() == "help")
                {
                    HelpCommand();
                }
                else//Неизвестная команды
                {
                    WriteLine("Неизвестная команда. Напишите help", ConsoleColor.Red);
                }

                Console.WriteLine();//Новая строка
            }
        }

        #region Команды сервера

        static void ShowAllClient()//Показать всех клиентов
        {
            WriteLine("Подождите...", ConsoleColor.Yellow);

            ConsoleTable table = new ConsoleTable("id", "nick", "email", "passworld");

            foreach (Data.ClientConnectOnly clientinfo in Data.ClientsOnlyData)//Проблема!
            {
                table.AddRow(1, 2, 3, 4).AddRow(clientinfo.ID, clientinfo.Nick.ToString(),
                    clientinfo.Email.ToString(),
                    clientinfo.Passworld.ToString());
            }
            table.AddRow(1, 2, 3, 4).AddRow("1", "Gladi", "gladi@gmail.com",
                    "АнтонЗлой!");

            table.Write();
        }

        private static void ShowCountClient()//Показать количество клиентов
        {
            WriteLine($"Всего клиентов в онлайн: {Data.ClientsOnlyData.Count}", ConsoleColor.White);
        }

        private static void HelpCommand()//Показывеет все команды (help)
        {
            WriteLine($"Версия сервера: {ver}\n", ConsoleColor.White);
            WriteLine("%---------Информация о создателей---------%", ConsoleColor.White);
            WriteLine("%---Gladi - Самый главный (коммунист)-----%", ConsoleColor.Yellow);
            WriteLine("%---Qliook - Антикоммунист----------------%", ConsoleColor.Green);
            WriteLine("%---sEKRETNY - Анимешник------------------%", ConsoleColor.Magenta);
            WriteLine("%-----------------------------------------%\n", ConsoleColor.White);

            WriteLine("exit - выход из сервера", ConsoleColor.White);
            WriteLine("client c - сколько всего клиентов подключено", ConsoleColor.White);
            WriteLine("client f - инфо о всех клиентов", ConsoleColor.White);
        }

        private static void ShowInfoClient(string id)//Информация о клиенте (по id)
        {

        }

        private static void ShowCountClientTable()//Показать количество клиентов (в таблице)
        {
            WriteLine("Подождите...", ConsoleColor.Yellow);

            var settings = (Settings)Data.Settings;
            Data.ClientConnectOffline[] clientsCheck = new Data.ClientConnectOffline[settings.LastId];
            for (int i = 0; i <= settings.LastId; i++)
            {
                clientsCheck[i] = Database.GetClientInfo(i);
            }
             
            ConsoleTable table = new ConsoleTable("id", "nick", "email", "passworld");//TODO: Остальные параметры

            foreach (Data.ClientConnectOffline clientinfo in clientsCheck)
            {
                table.AddRow(1, 2, 3, 4).AddRow(clientinfo.ID, clientinfo.Nick, clientinfo.Email,
                    clientinfo.Passworld);
            }

            table.Write();
        }

        private static void DisconnectClients()//Отключение всех клиентов
        {
            foreach (Data.ClientConnectOnly client in Data.ClientsOnlyData)
            {
                try
                {
                    client.ClientSocket.Close();
                    WriteLine($"Клиент {client.Email}, {client.Passworld} отключился", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    WriteLine($"Ошибка при отключение всех клиентво: {ex}", ConsoleColor.Red);
                }
            }
        }

        private static void OffServer()//Отключение сервера
        {
            WriteLine("Завершение работы...", ConsoleColor.Yellow);

            sw.Close();
            DisconnectClients();
            server.Stop();
            Environment.Exit(0);
        }

        #endregion

        #region Сообщения

        private static void MessagesMain(string text, Data.ClientConnectOnly clientinfo)//Отправка сообщения в общий чат
        {
            WriteLine($"Сообщение в общий чат: {text}", ConsoleColor.Green);
            byte[] buffer = new byte[1024];

            //%MME - Получение сообщение в общий чат

            buffer = Encoding.UTF8.GetBytes($"%MME:{text}:{clientinfo.Nick}:{clientinfo.ID}");

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

        static void MessagesDialog(string text, Data.ClientConnectOnly clientinfo, long chatID)/*Отправка сообщений
            отдельный чат*/
        {
            //TODO
        }

        #endregion

        #region Разное

        private static bool CheckClientOnly(long id)//проверка клиента на онлайн
        {
            bool check = false;//Для перебора и это ответ

            foreach (Data.ClientConnectOnly clientonly in Data.ClientsOnlyData)
            {
                if (clientonly.ID == id)
                {
                    check = true;
                }
            }

            if (check)
            {
                WriteLine($"Проверку на онлайн выполнил на (и нашёл) {id}",
    ConsoleColor.Green);
            }
            else
            {
                WriteLine($"Проверку на онлайн выполнил на (и НЕ нашёл) {id}",
    ConsoleColor.Green);

            }

            return check;
        }

        #endregion

        #region Управление клиентом

        private static void CheckConnectClients(object i)//Проверяет всех клиентов на подключение
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
                    WriteLine($"Получена команда {answer} от {onlyClient.Nick}:{onlyClient.ID}", 
                        ConsoleColor.Green);
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
                    try
                    {
                        Match regex = Regex.Match(answer, "%MES:(.*)");
                        string messagesText = regex.Groups[1].Value;

                        MessagesMain(messagesText, onlyClient);
                    }
                    catch
                    {
                        WriteLine($"Ошибка отправки сообщение в общий чат от {onlyClient.ID}", ConsoleColor.Red);
                        return;
                    }
                }
                else if (answer.Contains("%NCT"))//Новый чат
                {
                    //%NCT:idUser

                    try
                    {
                        Match regex = Regex.Match(answer, "%NCT:(.*)");

                        Database.CreateNewDialog(onlyClient.ID, long.Parse(regex.Groups[1].Value));

                        onlyClient.ClientSocket.Client.Send(Encoding.UTF8.GetBytes("1"));
                        WriteLine("Готово!", ConsoleColor.Green);
                    }
                    catch
                    {
                        WriteLine($"Ошибка при %NCT от {onlyClient.ID}:{onlyClient.Nick}",
                            ConsoleColor.Red);
                        return;
                    }
                }
                else if (answer.Contains("%MSE"))//Для отдельного чата (отправка)
                {
                    Match regex = Regex.Match(answer, "%MSE:(.*):(.*)");

                    //id чата
                    long idChat = long.Parse(regex.Groups[1].Value);

                    //Сообщение
                    string mess = regex.Groups[2].Value;

                    //Обработка

                    //Есть такой чат?

                    var set = (Settings)Data.Settings;
                    bool yesChat = false;
                    foreach (Data.IMessageInfoChat chatInfo in set.MessageInfoChats)
                    {
                        if (chatInfo.ID == idChat)
                        {
                            yesChat = true;
                        }
                    }

                    try
                    {
                        if (!yesChat)
                        {
                            WriteLine($"Есть такой чат! %MSE:{onlyClient.ID}", ConsoleColor.Red);
                        }
                        else
                        {
                            //Сама отправка сообщение
                            //Если клиент онлайн, то отправляем нему

                            long id1 = onlyClient.ID;
                            long id2;

                            //Поиск чата
                            //TODO
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"%MSE Ошибка: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else if (answer.Contains("%UPM"))//Отправить последнии сообщение (обновление сообщений) TODO
                {
                    try
                    {
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
                        return;
                    }
                }
                else if (answer.Contains("%EXI"))//Выход (отключение)
                {
                    try
                    {
                        onlyClient.ClientSocket.Close();
                        Data.ClientsOnlyData.Remove(onlyClient);
                        return;
                    }
                    catch
                    {
                        onlyClient.ClientSocket.Close();
                        Data.ClientsOnlyData.Remove(onlyClient);
                        WriteLine($"Ошибка клиента при выходе: {onlyClient.ID}:{onlyClient.Nick}", ConsoleColor.Red);
                        return;
                    }
                }
                else if (answer.Contains("%INF"))//Получить информацию о аккаунте
                {
                    try
                    {
                        int idClient = int.Parse(answer.Substring(5));//%INF:{id}

                        Data.ClientConnectOffline client = Database.GetClientInfo(idClient);

                        //INF:{id}:{name}:{status}:{email}

                        if (client.UserAvatar != Data.UserAvatar.Custom)
                        {
                            bool status = CheckClientOnly(client.ID);

                            buffer = Encoding.UTF8.GetBytes($"%INF:{client.ID}:{client.Nick}:{status}:" +
                                $"{client.Email}");

                            onlyClient.ClientSocket.Client.Send(buffer);
                        }
                        else
                        {
                            Data.ClientConnectOffline offline = Database.GetClientInfo(idClient);//Проверить!
                            //TODO: Вот тут просто отправлять аватарку

                        }
                        WriteLine($"Команда %INF от {onlyClient.Nick}:{onlyClient.ID} выполнена",
                            ConsoleColor.Green);
                    }
                    catch
                    {
                        WriteLine($"Ошибка в %INF от {onlyClient.ID}:{onlyClient.Nick}", ConsoleColor.Red);
                        return;
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
                    try
                    {
                        string stringTemp = $"%UUS:{Data.ClientsOnlyData.Count}:";

                        foreach (Data.ClientConnectOnly client in Data.ClientsOnlyData)
                        {
                            stringTemp += $"{client.Nick};{client.ID}:";
                        }
                        Console.WriteLine(stringTemp);

                        byte[] answerUUS = Encoding.UTF8.GetBytes(stringTemp);
                        //networkStream.Write(answerUUS, 0, answerUUS.Length);
                        onlyClient.ClientSocket.Client.Send(answerUUS);
                    }
                    catch
                    {
                        WriteLine($"Ошибка в %UUS от {onlyClient.ID}:{onlyClient.Nick}", ConsoleColor.Red);
                        return;
                    }
                }
                else if (answer.Contains("%UAT"))//Загрузка (обновление) аватарки клиента
                {
                    //%UAT:typeAvatar:sizeFile
                    Match regex = Regex.Match(answer, "%UAT:(.*):(.*)");
                    Data.UserAvatar userAvatar = (Data.UserAvatar)short.Parse(regex.Groups[1].Value);

                    if (File.Exists($@"Avatars\{onlyClient.ID}.png"))
                        File.Delete($@"Avatars\{onlyClient.ID}.png");

                    if (userAvatar == Data.UserAvatar.Custom)
                    { 
                        /*Инфа о файле
                        (Максимальный размер 8 мб)*/

                        int sizeFile = int.Parse(regex.Groups[2].Value);

                        //ДЛЯ ПОЛУЧЕНИЕ ФАЙЛОВ ПОРТ 909!!!
                        UdpClient udpClient = new UdpClient(909, AddressFamily.InterNetwork);
                        IPEndPoint end = (IPEndPoint)onlyClient.ClientSocket.Client.RemoteEndPoint;

                        //Загрузка файла
                        byte[] bufferFile = udpClient.Receive(ref end);
                        File.WriteAllBytes($@"Avatars\{onlyClient.ID}.png", bufferFile);
                        //TODO: Добавить в базу данных!
                    }
                    else
                    {
                        //TODO:Просто добавить в базу данных!
                    }

                    WriteLine($"Полученена аватарка от {onlyClient.ID}:{onlyClient.Nick}...",
    ConsoleColor.Yellow);
                }
                else//TODO: Это потом для API!
                {
                    try
                    {
                        //%ERR:CommandIsNot - Такой команды нет
                        onlyClient.ClientSocket.Client.Send(Encoding.UTF8.GetBytes("%ERR:CommandIsNot"));
                        WriteLine("Такой команды нет!", ConsoleColor.Red);
                    }
                    catch
                    {
                        WriteLine("Ошибка при отправке ОШИБКИ CommandIsNot", ConsoleColor.Red);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Новое подключение

        private static void ErrorConfirmData(TcpClient client, string email, string passworld)//Ошибка проверки данных
        {
            client.Client.Send(Encoding.UTF8.GetBytes("0"));
            WriteLine($"Ошибка при проверки данных у {email}:{passworld}", ConsoleColor.Red);
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

            //Провека проги
            try
            {
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

                Match regex = Regex.Match(answer, "%REG:(.*):(.*):(.*):(.*)");//Антон!
                string email = regex.Groups[1].Value;

                //string email = answer.
                //TODO: Сделать проверку email через подтверждение (Нужен smtp сервер)

                //пароль

                string passworld = regex.Groups[2].Value;//TODO: Нужен md5
                Console.WriteLine(passworld);

                //Nick

                string nick = regex.Groups[3].Value;
                Console.WriteLine(nick);

                //Аватарка

                int avatar = int.Parse(regex.Groups[4].Value);

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
                    Database.AccountAdd(email, passworld, nick, set.LastId, avatar, false);
                    client.Client.Send(Encoding.UTF8.GetBytes("%REGOD"));

                    WriteLine($"Новый аккаунт! {email}, {passworld}", ConsoleColor.Green);
                    Data.Settings = set;
                    SettingsManager.Save();

                    //Проверка подтверждения
                    //1 - есть подтверждение, 0 - нет (Но злой Антон всё переделал :) )

                    messi = client.Client.Receive(buffer);

                    answer = Encoding.UTF8.GetString(buffer, 0, messi);

                    if (answer == "1")
                    {
                    CheckDataConfirm:

                        //email

                        regex = Regex.Match(answer, "%LOG:(.*):(.*)");//Антон!
                        string emailCheck = regex.Groups[1].Value;

                        //пароль

                        string passworldCheck = regex.Groups[2].Value;

                        //Проверка
                        //1 - успешно, 0 - не успешно

                        if (emailCheck != email)
                        {
                            ErrorConfirmData(client, email, passworld);
                            goto CheckDataConfirm;
                        }
                        else if (passworldCheck != passworld)
                        {
                            ErrorConfirmData(client, email, passworld);
                            goto CheckDataConfirm;
                        }
                        else
                        {
                            client.Client.Send(Encoding.UTF8.GetBytes("1"));
                        }
                    }

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
                            Data.UserAvatar.Avatar1, false);//TODO

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

        #endregion

        #region Дизайн

        private static void WriteLine(string text, ConsoleColor color)//Кравивый текст и логи
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            sw.WriteLine($"{DateTime.Now}: {text}");
        }

        #endregion
    }
}