using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;



public enum GameLobbySize
{
    SOLO = 0,
    DUO = 1,
    QUAD = 2
}

public class Local_Com : MonoBehaviour
{
    float timer = 3f;
    public bool runWithServer = true;
    public GameLobbySize lobbySize = GameLobbySize.SOLO;
    private string[] sizeArgs = new string[] { "SOLO", "DUO", "QUAD" };
    // Start is called before the first frame update
    void Start()
    {
#if(UNITY_EDITOR)
        if (runWithServer) ClientRoot.StartDebugServer(sizeArgs[(int)lobbySize]);
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (ClientRoot.isConnected)
        {
            ClientRoot.CheckSocket();
        }
        else if(runWithServer)
        {
            ConnectionInitWait();
        }
#else
        if(ClientRoot.isConnected){
            ClientRoot.CheckSocket();
        }
        else{
            ConnectionInitWait();
        }
#endif

    }



    public void DebugDisconnect()
    {
        ClientRoot.StopDebugServer();
    }

    //change this to an Await() when you feel less stupid
    private void ConnectionInitWait()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                ClientRoot.ConnectToOwnServer();
            }
        }

    }

    private void OnDestroy()
    {
        ClientRoot.StopDebugServer();
    }
}
