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
    public Text ErrorBoxMessage;

    public GameObject[] menu; //0-register 1-logni 2-chat

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
    }

    private void OnIncomingData()
    {

        if (message.Contains("%REGOD"))
        {
            message.Substring(7);
            ErrorBoxMessage.text = message;
            menu[0].active = false;
            menu[1].active = true;
            menu[2].active = false;
        }
        if (message.Contains("%LOGOD"))
        {
            message.Substring(7);
            ErrorBoxMessage.text = message;
            menu[0].active = false;
            menu[1].active = false;
            menu[2].active = true;
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

        Debug.Log(Email + Pass + clientName);
        Send($"%REG:{Email}:{Pass}:{clientName}");
        Task.Delay(10).Wait();
        return;
    }

    public void StartLogin()
    {
        Email = GameObject.Find("LoginInput").GetComponent<InputField>().text;
        Pass = GameObject.Find("LPassInput").GetComponent<InputField>().text;
        GameObject.Find("LPassInput").GetComponent<InputField>().text = "";

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


