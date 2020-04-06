using System.Collections;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Client : MonoBehaviour
{
    public GameObject chatContainer;
    public GameObject messagePrefab;

    public GameObject[] menu; //0-register 1-logni 2-chat
    public GameObject MesBox;
    public Text MessageBox;
    public GameObject NPS;

    private string clientName;
    private string Email;
    private string Pass;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string message;

    public void ConnectedToServer()
    {
        //if already connection, igmore this function
        if (socketReady)
            return;

        //Defalt host / post values
        string host = "127.0.0.1";
        int port = 908;

        
        //Create the socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);//Это только для общего чата получать
            reader = new StreamReader(stream);//Или для беседы
            socketReady = true;
            Send("TCPCHAT 1.0");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    bool reg = false;
    bool log = false;
    bool streg = false;
    float time = 1;

    private void Start()
    {
        ConnectedToServer();
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                socket.Client.Receive(buffer);

                message = Encoding.UTF8.GetString(buffer);
                OnIncomingData();
                Debug.Log(message);          
                    
            }
        }

        if (streg)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                menu[0].active = true;
                menu[1].active = false;
                menu[2].active = false;
                NPS.active = true;
                MesBox.active = true;             
                time = 1;
                streg = false;
            }
        }
        if (reg)
        {          
            time -= Time.deltaTime;
            if(time <=0)
            {
                NPS.active = false;
                MesBox.active = false; 
                menu[0].active = false;
                menu[1].active = true;
                menu[2].active = false;

                time = 1;
                reg = false;
            }
        }
        if (log)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                menu[0].active = false;
                menu[1].active = false;
                menu[2].active = true;              
                GameObject.Find("Reg_Log_Image").active = false;

                time = 1;
                log = false;
            }
        }
    }

    public void GoToRegistration()
    {
        GameObject.Find("Image_change_reg_log").GetComponent<Animation>().Play("Change_reg_log");
        MessageBox.text = "Введите свои данные";
        streg = true;
        
    } //streg | Если не зарегистрировались
   
    public void GoToLogin()
    {
        GameObject.Find("Image_change_reg_log").GetComponent<Animation>().Play("Change_reg_log");
        reg = true;
    }

    private void OnIncomingData()
    {

        if (message.Contains("%REGOD"))
        {                    
            GameObject.Find("Image_change_reg_log").GetComponent<Animation>().Play("Change_reg_log");
            reg = true;
        } //Переход к логину
        if (message.Contains("%LOGOD"))
        {
            GameObject.Find("Image_change_reg_log").GetComponent<Animation>().Play("Change_reg_log");
            log = true;
        } //Переход к чату

        if (message.Contains("%REGWRONGEMAIL"))
        {
            NPS.GetComponent<Animation>().Play("Wrong_Email");
            GameObject.Find("EmailInput").GetComponent<InputField>().text = "";
            MessageBox.text = "Данный email уже зарегистрирован";
        }
        if (message.Contains("%LOGWRONGEMAIL") || message.Contains("%LOGWRONGEPASS"))
        {
            MessageBox.text = "Данные введены неверно";
            GameObject.Find("LPassInput").GetComponent<InputField>().text = "";
        }

        GameObject go = Instantiate(messagePrefab, chatContainer.transform);
        go.GetComponentInChildren<Text>().text = message;
        Destroy(go, 10);
    }

    //\\ 
    public void StartRegistration()
    {
        Email = GameObject.Find("EmailInput").GetComponent<InputField>().text;
        Pass = GameObject.Find("PassInput").GetComponent<InputField>().text;
        clientName = GameObject.Find("NickInput").GetComponent<InputField>().text;

        Debug.Log("Данные отправленные для регистрации: " + Email + " " + Pass + " " + clientName);
        Send($"%REG:{Email}:{Pass}:{clientName}");
        Task.Delay(10).Wait();
        return;
    }

    public void StartLogin()
    {
        Email = GameObject.Find("LoginInput").GetComponent<InputField>().text;
        Pass = GameObject.Find("LPassInput").GetComponent<InputField>().text;
        GameObject.Find("LPassInput").GetComponent<InputField>().text = "";

        Debug.Log("Данные отправленные для входа: " + Email + " " + Pass);
        Send($"%LOG:{Email}:{Pass}");
        Task.Delay(10).Wait();
        return;
    }
    //\\

    private void Send(string data)
    {
        if (!socketReady)
            return;

        socket.Client.Send(Encoding.UTF8.GetBytes(data));
        writer.Flush();
    }

    public void OnSendButton()
    {
        string message = GameObject.Find("SendInput").GetComponent<InputField>().text;
        GameObject.Find("SendInput").GetComponent<InputField>().text = "";
        Send(message);
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }
}


