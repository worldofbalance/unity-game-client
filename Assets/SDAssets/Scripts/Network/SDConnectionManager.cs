using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Net.Sockets;

namespace SD
{ 
    public class SDConnectionManager : MonoBehaviour
    {


       private GameObject mainObject;
       private TcpClient mySocket;
       private NetworkStream theStream;
       private bool socketReady = false;

       static private SDConnectionManager singleInstance;

       public static SDConnectionManager getInstance()
    {
            
            return singleInstance;
    }

    void Awake()
    {
        singleInstance = this;
        mainObject = GameObject.Find("MainObject");
            
        }

    // Use this for initialization
    void Start()
    {
        SetupSocket();
    }

    public void SetupSocket()
    {
        if (socketReady)
        {
            Debug.Log("Already Connected");
            return;
        }

        try
        {
            mySocket = new TcpClient(Config.REMOTE_HOST, SD.Constants.REMOTE_PORT);
            theStream = mySocket.GetStream();
            socketReady = true;

            Debug.Log("Connected");


        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }

    public void ReadSocket()
    {
        if (!socketReady)
        {
            return;
        }

        if (theStream.DataAvailable)
        {
            byte[] buffer = new byte[2];
            theStream.Read(buffer, 0, 2);
            short bufferSize = BitConverter.ToInt16(buffer, 0);

            buffer = new byte[bufferSize];
            theStream.Read(buffer, 0, bufferSize);
            MemoryStream dataStream = new MemoryStream(buffer);

            short response_id = DataReader.ReadShort(dataStream);
            Debug.Log("response_id: " + response_id);
            //Debug.Log(response_id.GetType().ToString());
            NetworkResponse response = NetworkResponseTable.get(response_id);

            if (response != null)
            {
                response.dataStream = dataStream;

                response.parse();
                ExtendedEventArgs args = response.process();

                if (args != null)
                {
                   SDMessageQueue msgQueue = SDMessageQueue.getInstance();
                   msgQueue.AddMessage(args.event_id, args);
                }
            }
        }
    }

    public void CloseSocket()
    {
        if (!socketReady)
        {
            return;
        }
        mySocket.Close();
        socketReady = false;
    }

    public void Send(NetworkRequest request)
    {
        if (!socketReady)
        {
            return;
        }

        GamePacket packet = request.packet;

       byte[] bytes = packet.getBytes();
       theStream.Write(bytes, 0, bytes.Length);

      // if (request.request_id != Constants.CMSG_HEARTBEAT) {
       Debug.Log("Sent Request No. " + request.request_id + " [" + request.ToString() + "]");
      //  }
    }

    // Update is called once per frame
    void Update()
    {
        ReadSocket();
    }
}
}
