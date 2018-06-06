using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Networking;
using SharedTypes;
using SharedMessages;

public class MyNetworkManager : NetworkManager
{
    private ConfigSingleton _config;
    private void Start()
    {
        _config = ConfigSingleton.GetInstance();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Got connection with server!");
        singleton.client.RegisterHandler(MyMsgType.TestCases, GetConfiguration);
        singleton.client.RegisterHandler(MyMsgType.UserList, GetUserList);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (conn.lastError == NetworkError.Timeout)
        {
            StopClient();
            Debug.Log("Attempting recconect.");
            StartClient();
        }
        else
        {
            base.OnClientDisconnect(conn);
        }
    }

    public void GetUserList(NetworkMessage message)
    {
        UserListMessage msg = message.ReadMessage<UserListMessage>();
        GameObject.Find("GameController(Clone)").GetComponent<GameManager>().GotReturningUsers(msg.UserList);
    }

    public void GetConfiguration(NetworkMessage message)
    {
        TestCasesMessage msg = message.ReadMessage<TestCasesMessage>();
        _config.SetTestCases(msg.TestCases);
        FindObjectOfType<MainLoop>().GotConfig();
    }

    private void OnApplicationQuit()
    {
        StopClient();
    }
}
