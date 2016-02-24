using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

public class ConnectionManager {

	// Status Codes
	public static readonly short SUCCESS = 0;
	public static readonly short FAILED = 1;
	public static readonly short CONNECTED = 2;
	// Variables
	private TcpClient mySocket = new TcpClient();
	public bool Connected { get { return mySocket.Connected; } }
	private NetworkStream theStream;
	private string hostname;
	private int port;
	
	public short Connect(string hostname, int port) {
		this.hostname = hostname;
		this.port = port;

		if (mySocket.Connected) {
			Debug.Log("Already Connected");
			return CONNECTED;
		}

		try {
			mySocket = new TcpClient(hostname, port);
			theStream = mySocket.GetStream();

			Debug.Log("Connected");
			return SUCCESS;
		} catch (Exception e) {
			Debug.Log("Connection Failed");
		}

		return FAILED;
	}

	public short Reconnect() {
		Debug.Log("Reconnecting...");
		return Connect(hostname, port);
	}
	
	public void Close() {
		if (!mySocket.Connected) {
			return;
		}
		
		mySocket.Close();
	}

	public List<NetworkResponse> Read(int numPackets = 20) {
		List<NetworkResponse> responses = new List<NetworkResponse>();

		while (theStream.DataAvailable && responses.Count < numPackets) {
			byte[] buffer = new byte[2];
			theStream.Read(buffer, 0, 2);
			short bufferSize = BitConverter.ToInt16(buffer, 0);

			buffer = new byte[bufferSize];
			//to allow for network latency, check number of bytes read and continue reading
			//until expected data is received
			int bytesRead = 0;
			int counter = 0;
			do{
				bytesRead += theStream.Read(buffer, bytesRead, bufferSize - bytesRead);
				counter++;
			}
			while(bytesRead < bufferSize);
			MemoryStream dataStream = new MemoryStream(buffer);

			short protocol_id = DataReader.ReadShort(dataStream);
			Type pType = NetworkProtocolTable.Get(protocol_id);
			if (counter > 1) {
				Debug.Log (string.Format (
					"Note, network latency issue identified, wait count = {0}, protocol ID = {1}", 
					counter,
					protocol_id
					));
			}
			//output packet-level info to screen for debugging purposes
			//DebugPacket (buffer, true, false);

			if (pType == null) {
				Debug.LogError("Invalid Response No. " + protocol_id + " [" + "Unknown" + "]");
			} else {
				try {
					NetworkResponse args = pType.GetMethod("Parse").Invoke(null, new object[]{dataStream}) as NetworkResponse;

					if (args != null) {
						responses.Add(args);
					}
				} catch (Exception ex) {
					Debug.LogError("Failed Response No. " + protocol_id + " [" + pType.ToString() + "]");
					Debug.LogException(ex);
				}
			}
		}

		return responses;
	}
	
	public bool Send(byte[] bytes) {
		try {
			theStream.Write(bytes, 0, bytes.Length);
			//output packet-level info to screen for debugging purposes
			//DebugPacket (bytes, true, true);
			return true;
		} catch (IOException ex) {
			Debug.LogError("Failed to send");
		}

		return false;
	}

	private void DebugPacket(byte[] bytes, bool force, bool outbound) {
		int limit = 30;
		if (bytes [2] > 149 || force) {
			Debug.Log (string.Format (
				"ConnectionManager::DebugPacket(), {0}: {1}, 1st {2} bytes of {3}...",
				(outbound ? "SEND" : "RCV"), 
				(outbound ? bytes [2] & 0x000000FF : bytes [0] & 0x000000FF),
				limit,
			    bytes.Length
			    ));
			for (int i = 0; i < Math.Min(bytes.Length, limit); i++) {
				byte [] b = new byte[1];
				b [0] = bytes [i];
				Debug.Log (string.Format ("byte {0}, hx {1}, dc {2}, asc '{3}'", 
				                          i, b [0].ToString ("X2"), b [0] & 0x000000FF, 
				                          System.Text.Encoding.ASCII.GetString(b)));
				
			}
			if (bytes.Length > limit) {
				byte[] remainder = new byte [bytes.Length - limit];
				for (int i = limit; i < bytes.Length; i++) {
					remainder[i - limit] = bytes[i];
				}
				Debug.Log (string.Format (
					"Remainder {0}: {1}", 
					(outbound ? bytes [2] & 0x000000FF : bytes [0] & 0x000000FF),
					System.Text.Encoding.ASCII.GetString(remainder)
					));
			}
		}
	}

}
