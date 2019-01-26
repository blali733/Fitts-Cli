using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Networking;
using FittsLibrary;
using FittsLibrary.Messages;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkManager : NetworkManager
{
    public static bool IsConnected = false;
    private ConfigSingleton _config;
    private void Start()
    {
        _config = ConfigSingleton.GetInstance();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Got connection with server!");
//        IsConnected = true;
        singleton.client.RegisterHandler(MyMsgType.TestCases, GetConfiguration);
        singleton.client.RegisterHandler(MyMsgType.UserList, GetUserList);
        singleton.client.RegisterHandler(MyMsgType.DeviceId, GotDevId);
        singleton.client.RegisterHandler(MyMsgType.ColorRanges, GotColorSpaces);
        singleton.client.RegisterHandler(MyMsgType.UserCode, GotUserCodeMessage);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (conn.lastError == NetworkError.Timeout && !IsConnected)
        {
            StopClient();
            GameObject.Find("MainLoopController").gameObject.GetComponent<MainLoop>().ConnectionFailed();
        }
        else
        {
            StopClient();
            Debug.Log("Reconnecting...");
            singleton.StartClient();
        }
    }

    public void GotUserCodeMessage(NetworkMessage message)
    {
        GameObject.Find("GameController(Clone)").GetComponent<GameManager>().GotUserCode(message.ReadMessage<StringMessage>().value);
    } 

    public void GotColorSpaces(NetworkMessage message)
    {
        if (!IsConnected)
        {
            _config.ColorRanges = message.ReadMessage<ColorRangesMessage>().ColorRangeList;
            GameObject.Find("MainLoopController").gameObject.GetComponent<MainLoop>().GotColorSpace();
        }
    }

    public void GotDevId(NetworkMessage message)
    {
        if (!IsConnected)
        {
            int id = message.ReadMessage<IntegerMessage>().value;
            _config.DevId = id;
            GameObject.Find("MainLoopController").gameObject.GetComponent<MainLoop>().GotDevID();
        }
    }

    public void GetUserList(NetworkMessage message)
    {
        UserListMessage msg = message.ReadMessage<UserListMessage>();
        GameObject.Find("GameController(Clone)").GetComponent<GameManager>().GotReturningUsers(msg.UserList);
    }

    public void GetConfiguration(NetworkMessage message)
    {
        if (!IsConnected)
        {
            TestCasesMessage msg = message.ReadMessage<TestCasesMessage>();
            _config.SetTestCases(msg.TestCases);
            FindObjectOfType<MainLoop>().GotConfig();
        }
    }

    private void OnApplicationQuit()
    {
        StopClient();
    }
}
