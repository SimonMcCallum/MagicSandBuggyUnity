using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    public string IP = "localhost";
    public Int32 port = 9966;

    private TcpClient client;
    private NetworkStream stream;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openConnection()
    {
        Debug.Log("Trying to Open a Connection to " + IP + ":" + port);
        client = new TcpClient(IP, port);
        // Get a client stream for reading and writing.
        stream = client.GetStream();
        //will be closed on destroy
    }

    public void echo()
    {
        if (stream == null)
        {
            Debug.Log("open conection before sending");
            return;
        }
        
        Byte[] data = new Byte[257];
        data[0] = 1; //leading byte see server doc for protocoll
        // Translate the passed message into ASCII and store it as a Byte array.
        System.Text.Encoding.ASCII.GetBytes("Test Unity Connection").CopyTo(data,1);

        Debug.Log("Sending echo request");
        stream.Write(data, 0, data.Length);

        // String to store the response ASCII representation.
        String responseData = String.Empty;

        // Read the first batch of the TcpServer response bytes.
        Int32 bytes = stream.Read(data, 0, data.Length);
        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        Debug.Log("Received: " + responseData);

    }

    private void OnDestroy()
    {
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
    }
}
