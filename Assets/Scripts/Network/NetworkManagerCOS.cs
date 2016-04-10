using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class NetworkManagerCOS : NetworkAbstractManager
{
    private static NetworkManagerCOS instance;

    public static NetworkManagerCOS getInstance()
    {
        if (instance == null)
            throw new Exception("NetworkManagerCOS awake not called");
        else
            return instance;
    }

    void Awake()
    {
        instance = this;
        netProtTable = new NetworkProtocolTableCOS();
        cManager = new ConnectionManager(netProtTable);
        Listen(NetworkCode.HEARTBEAT, ProcessHeartbeat);
    }
	
    // Use this for initialization
    void Start()
    {
        
        if (cManager.Connect(Config.REMOTE_HOST, Constants.REMOTE_PORT_COS) == ConnectionManager.SUCCESS)
        {
            Send(
                ClientProtocol.Prepare(Constants.CLIENT_VERSION, Constants.SESSION_ID_COS),
                ProcessClient
            );
        }
        StartCoroutine(Poll(Constants.HEARTBEAT_RATE));
    }

    protected override String toString()
    {
        return "NetworkManagerCOS";
    }

    public override void ProcessClient(NetworkResponse response)
    {
        ResponseClient args = response as ResponseClient;
        Constants.SESSION_ID_COS = args.session_id;
    }
}
