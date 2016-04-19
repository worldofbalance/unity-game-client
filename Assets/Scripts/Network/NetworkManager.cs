using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class NetworkManager {

	public delegate void Callback(NetworkResponse response);
	private Dictionary<int, Queue<Callback>> callbackList;
	private Dictionary<int, List<Callback>> listenList;
	private MonoBehaviour owner;
	// Connection
	private ConnectionManager cManager;
	private Queue<NetworkRequest> requests;
	private int counter = 0;
	private int interval = 50;

	private static bool lostConnection = false;

	// The NetworkManager needs a MonoBehaviour instance to start co-routines on
	public NetworkManager(MonoBehaviour owner, ConnectionManager cManager) {
		this.owner = owner;
		this.cManager = cManager;
		this.requests = new Queue<NetworkRequest>();
		this.callbackList = new Dictionary<int, Queue<Callback>>();
		this.listenList = new Dictionary<int, List<Callback>>();
		
		NetworkProtocolTable.Init();

		Listen (NetworkCode.HEARTBEAT, ProcessHeartbeat);

		if (cManager.Connect() == ConnectionManager.SUCCESS) {
			Send(
				ClientProtocol.Prepare(Constants.CLIENT_VERSION, Constants.SESSION_ID),
				ProcessClient
			);
		}

		owner.StartCoroutine(Poll(Constants.HEARTBEAT_RATE));
	}
	
	// Update should be called within a Game's Update method
	public void Update () {
		if (!cManager.Connected) {
			return;
		}

		while (requests.Count > 0) {
			NetworkRequest packet = requests.Peek();

			if (cManager.Send(packet.GetBytes())) {
				requests.Dequeue();
                // commented by Rujoota
				//Debug.Log("Sent Request No. " + packet.GetID() + " [" +  NetworkProtocolTable.Get(packet.GetID()).ToString() + "]");
			}
		}

		counter++;
		if (counter == interval) {
			counter = 0;
		}

		foreach (NetworkResponse args in cManager.Read()) {
			bool status = false;

			int protocol_id = args.GetID();

			// One-Time
			if (callbackList.ContainsKey(protocol_id)) {
				if (callbackList[protocol_id].Count > 0) {
					callbackList[protocol_id].Dequeue()(args);

					status = true;
				}
			}
			// Listen
			if (listenList.ContainsKey(protocol_id)) {
				if (listenList[protocol_id].Count > 0) {
					foreach (Callback callback in listenList[protocol_id]) {
						callback(args);
					}

					status = true;
				}
			}

			/*Debug.Log((status ? "Processed" : "Ignored") + " Response No. " + 
			          args.GetID() + " [" + NetworkProtocolTable.Get(args.GetID()).ToString() + "]");*/
		}
	}

	public void Send(NetworkRequest packet) {
		requests.Enqueue(packet);
	}
	
	public void Send(NetworkRequest packet, Callback callback) {
		this.Send(packet);

		int protocol_id = packet.GetID();
		if (!callbackList.ContainsKey(protocol_id)) {
			callbackList[protocol_id] = new Queue<Callback>();
		}

		callbackList[protocol_id].Enqueue(callback);
	}

	public void Listen(int protocol_id, Callback callback) {
		if (!listenList.ContainsKey(protocol_id)) {
			listenList[protocol_id] = new List<Callback>();
		}

		listenList[protocol_id].Add(callback);
	}

	public void Ignore(int protocol_id, Callback callback) {
		if (listenList.ContainsKey(protocol_id) && listenList[protocol_id].Contains(callback)) {
			while (listenList[protocol_id].Contains(callback)) {
				listenList[protocol_id].Remove(callback);
			}
		} else {
			Debug.LogError("Callback for Protocol [" + protocol_id + "] Does Not Exist");
		}
	}

	public void Clear() {
		callbackList.Clear();
		listenList.Clear();
		requests.Clear();
	}
	
	private IEnumerator Poll(float time) {
		while (true) {
			if (cManager.Connected) {
				cManager.Send(HeartbeatProtocol.Prepare().GetBytes());
				yield return new WaitForSeconds(time);
			} else {
				cManager.Reconnect();
				yield return new WaitForSeconds(3.0f);
			}
		}
	}
	
	public void ProcessClient(NetworkResponse response) {
		ResponseClient args = response as ResponseClient;
		Constants.SESSION_ID = args.session_id;
	}

	public void ProcessHeartbeat(NetworkResponse response) {
		if (!lostConnection) {
			Debug.LogWarning ("Lost connection!");

			owner.gameObject.AddComponent <ConnectionLostGUI>();

			lostConnection = true;
		}
	}
}
