using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour {


    int connectionId;
    int channelId;
    int socketId;

    public int connectPort;
    public int socketPort = 8888;

	// Use this for initialization
	void Start () {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        //send data, but not in order;
        channelId = config.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(config, 64);

        socketId = NetworkTransport.AddHost(topology, socketPort);

	}
	
	// Update is called once per frame
	void Update () {
        int outHostId;
        int outConnectionId;
        int outChannelId;
        
        int dataSize;
        byte error;

        // 000 0000 0000 0000 | each 0 represents a byte 
        byte[] recBuffer = new byte[4096];
        int bufferSize = 4096;

        //this line is broken, don't ask about it yet
        NetworkEventType evt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, recBuffer, bufferSize, out dataSize, out error);

        switch (evt)
        {
            case NetworkEventType.ConnectEvent:
                OnConnection(outHostId, outConnectionId, (NetworkError)error);
                break;
            case NetworkEventType.DisconnectEvent:
                OnDiscconnect(outHostId, outConnectionId, (NetworkError)error);
                break;
            case NetworkEventType.DataEvent:
                OnData(outHostId, outConnectionId, outChannelId, recBuffer, dataSize, (NetworkError)error);
                break;
            case NetworkEventType.BroadcastEvent:
                OnBroadcast(outHostId, recBuffer, dataSize, (NetworkError)error);
                break;
            case NetworkEventType.Nothing:
                break;
            default:
                Debug.LogError("Unkown network message type received: " + evt);
                break;
        }
	}

    public void Connect() {
        byte error;
        connectionId = NetworkTransport.Connect(socketId, "localhost", connectPort, 0, out error);
        Debug.Log("Connected to server. ConnectionId: " + connectionId);
    }

    public void SendSocketMessage() {
        byte error;
        byte[] buffer = new byte[4096];
        int bufferSize = 4096;

        Stream stream = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, "Hello World");


        NetworkTransport.Send(socketId, connectionId, channelId, buffer, bufferSize, out error);

    }

    void OnConnection(int hostId, int connectionId, NetworkError error) {
        Debug.Log("OnConnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

    void OnDiscconnect(int hostId, int connectionId, NetworkError error) {
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

    void OnBroadcast(int hostId, byte[] data, int size, NetworkError error) {
        Debug.Log("OnBroadcast(hostId = " + hostId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");
    }

    void OnData(int hostId, int connectionId, int channelId, byte[] data, int size, NetworkError error) {

        Stream stream = new MemoryStream(data);
        BinaryFormatter formatter = new BinaryFormatter();
        string message = formatter.Deserialize(stream) as string; 

        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", channelId = " + channelId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");

        Debug.Log("Incoming message event: " + message);
    }
}
