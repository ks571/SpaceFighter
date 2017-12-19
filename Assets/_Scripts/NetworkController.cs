using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour {


    int connectionId;
    int channelId;
    int hostId;

    public string connectIp;
    public int connectPort;

	// Use this for initialization
	void Start () {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        //send data, but not in order;
        channelId = config.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(config, 64);

        hostId = NetworkTransport.AddHost(topology, 12345);

        byte error;
        connectionId = NetworkTransport.Connect(hostId, connectIp, connectPort, 0, out error);
	}
	
	// Update is called once per frame
	void Update () {
        int outHostId;
        int outConnectionId;
        int outChannelId;
        
        int receivedSize;
        byte error;

        // 000 0000 0000 0000 | each 0 represents a byte 
        byte[] buffer = new byte[4096];

        //this line is broken, don't ask about it yet
        NetworkEventType evt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);

        switch (evt)
        {
            case NetworkEventType.ConnectEvent:
                OnConnection(outHostId, outConnectionId, (NetworkError)error);
                break;
            case NetworkEventType.DisconnectEvent:
                OnDiscconnect(outHostId, outConnectionId, (NetworkError)error);
                break;
            case NetworkEventType.DataEvent:
                OnData(outHostId, outConnectionId, outChannelId, buffer, receivedSize, (NetworkError)error);
                break;
            case NetworkEventType.BroadcastEvent:
                OnBroadcast(outHostId, buffer, receivedSize, (NetworkError)error);
                break;
            case NetworkEventType.Nothing:
                break;
            default:
                Debug.LogError("Unkown network message type received: " + evt);
                break;
        }
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
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", channelId = " + channelId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");
    }
}
