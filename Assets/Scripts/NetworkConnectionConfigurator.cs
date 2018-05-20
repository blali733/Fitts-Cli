﻿using System;
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
        NetworkManager.singleton.networkAddress = _networkConfig.Address;
        NetworkManager.singleton.networkPort = int.Parse(_networkConfig.Port);
        NetworkManager.singleton.StartClient();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
