using System.Collections;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;

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
                if (buffer != null)
                {
                    OnIncomingData(buffer);
                    Debug.Log(Encoding.UTF8.GetString(buffer));
                }
            }
        }
    }

    private void OnIncomingData(byte[] buffer)
    {
        string data = Encoding.UTF8.GetString(buffer);


        if (data.Contains("%REGOD"))
        {
            data.Substring(7);
            ErrorBoxMessage.text = data;
            menu[0].active = false;
            menu[1].active = true;
            menu[2].active = false;
            Debug.Log(data);
        }

        GameObject go = Instantiate(messagePrefab, chatContainer.transform);
        go.GetComponentInChildren<Text>().text = data;
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
        return;
    }

    public void StartLogin()
    {
        string Email = GameObject.Find("EmailInput").GetComponent<InputField>().text;
        Pass = GameObject.Find("PassInput").GetComponent<InputField>().text;
        GameObject.Find("PassInput").GetComponent<InputField>().text = "";

        Send($"%LOG:{Email}:{Pass}");
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
