using System;
using System.Collections;
using System.Collections.Generic;
using SharedTypes;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkConnectionConfigurator : MonoBehaviour
{
    private MyNetworkConfig _networkConfig;
    private bool _started;
    private ConfigSingleton _config;

    // Use this for initialization
    void Start()
    {
        _config = ConfigSingleton.GetInstance();
        _started = false;
    }

    public void SetupConnection()
    {
        _networkConfig = _config.GetMyNetworkConfig();
        MyNetworkManager.singleton.networkAddress = _networkConfig.Address;
        MyNetworkManager.singleton.networkPort = int.Parse(_networkConfig.Port);
        MyNetworkManager.singleton.StartClient();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
