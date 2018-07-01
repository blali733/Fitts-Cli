using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Networking;
using SharedTypes;
using SharedMessages;
using UnityEngine.Networking.NetworkSystem;

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
        singleton.client.RegisterHandler(MyMsgType.DeviceId, GotDevId);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (conn.lastError == NetworkError.Timeout)
        {
            StopClient();
            GameObject.Find("MainLoopController").gameObject.GetComponent<MainLoop>().ConnectionFailed();
        }
        else
        {
            base.OnClientDisconnect(conn);
        }
    }

    public void GotDevId(NetworkMessage message)
    {
        int id = message.ReadMessage<IntegerMessage>().value;
        _config.DevId = id;
        GameObject.Find("MainLoopController").gameObject.GetComponent<MainLoop>().GotDevID();
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
