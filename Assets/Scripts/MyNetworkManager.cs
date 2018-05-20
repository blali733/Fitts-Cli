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
        singleton.client.RegisterHandler(MyMsgType.TestCases, GetConfiguration);
    }

    public void GetConfiguration(NetworkMessage message)
    {
        TestCasesMessage msg = message.ReadMessage<TestCasesMessage>();
        _config.SetTestCases(msg.TestCases);
        FindObjectOfType<MainLoop>().GotConfig();
    }


}
