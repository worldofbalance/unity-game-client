using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class NetworkManagerCOS : NetworkAbstractManager
{
    private static NetworkManagerCOS instance;
    private bool isLoggedInToCosServer = false;
    private bool retryOnce = true;


    public bool IsLoggedInToCosServer {
        get { return isLoggedInToCosServer; }
        set { isLoggedInToCosServer = value; }
    }

    public static NetworkManagerCOS getInstance ()
    {
        if (instance == null)
            throw new Exception ("NetworkManagerCOS awake not called");
        else
            return instance;
    }

    void Awake ()
    {
        instance = this;
        netProtTable = new NetworkProtocolTableCOS ();
//        cManager = new ConnectionManager (Constants.REMOTE_HOST, Constants.REMOTE_PORT_COS);
        cManager = new ConnectionManager (Config.REMOTE_HOST, Constants.REMOTE_PORT_COS);
        Listen (NetworkCode.HEARTBEAT, ProcessHeartbeat);
    }
	
    // Use this for initialization
    void Start ()
    {
        
        if (cManager.Connect () == ConnectionManager.SUCCESS) {
            Send (
                ClientProtocol.Prepare (Constants.CLIENT_VERSION, Constants.SESSION_ID_COS),
                ProcessClient
            );
        }
        StartCoroutine (Poll (Constants.HEARTBEAT_RATE));
    }

    protected override String toString ()
    {
        return "NetworkManagerCOS";
    }

    public override void ProcessClient (NetworkResponse response)
    {
        ResponseClient args = response as ResponseClient;
        Constants.SESSION_ID_COS = args.session_id;

        sendLoginReq ();
    }

    public void ProcessLogin (NetworkResponse response)
    {
        ResponseLogin args = response as ResponseLogin;

        if (args.status == 0) {
            Debug.Log ("Succesfully logged to COS server" + args.status);
            isLoggedInToCosServer = true;
        } else {
            Debug.Log ("login to cos server failed, server message = " + args.status);
            isLoggedInToCosServer = false;
            if (retryOnce) {
                retryOnce = false;
                sendLoginReq ();
            }

            //mainObject.GetComponent<Main>().CreateMessageBox("Login Failed");
        }
    }

    void sendLoginReq ()
    {
        Send (
            LoginProtocol.Prepare (GameState.account.username, GameState.account.password),
            //                    LoginProtocol.Prepare("dule", "qwerty"),
            ProcessLogin
        );
    }
}
