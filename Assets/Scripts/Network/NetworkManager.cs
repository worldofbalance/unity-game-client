using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class NetworkManager : NetworkAbstractManager
{

    private static NetworkManager instance;

    public static NetworkManager getInstance()
    {
        if (instance == null)
            throw new Exception("NetworkManager awake not called");
        else
            return instance;
    }

    protected override String toString()
    {
        return "NetworkManager  used for login";
    }

    void Awake()
    {
        instance = this;
        netProtTable = new NetworkProtocolTable();
        cManager = new ConnectionManager(netProtTable);
        Listen(NetworkCode.HEARTBEAT, ProcessHeartbeat);
    }
	
    // Use this for initialization
    void Start()
    {
        if (cManager.Connect(Config.REMOTE_HOST, Constants.REMOTE_PORT) == ConnectionManager.SUCCESS)
        {
            Send(
                ClientProtocol.Prepare(Constants.CLIENT_VERSION, Constants.SESSION_ID),
                ProcessClient
            );
        }
        StartCoroutine(Poll(Constants.HEARTBEAT_RATE));
    }

    public override void ProcessClient(NetworkResponse response)
    {
        ResponseClient args = response as ResponseClient;
        Constants.SESSION_ID = args.session_id;
    }
	
}
