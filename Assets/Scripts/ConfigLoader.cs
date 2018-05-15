using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SharedTypes;

[Serializable]
public class ConfigContent
{
    public string serverIP;
    public string serverPort;
}

public class ConfigLoader : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        string config = LoadFile();
	    ConfigContent configuration = JsonUtility.FromJson<ConfigContent>(config);
        ConfigSingleton configInstance = ConfigSingleton.GetInstance();
        configInstance.SetMyNetworkConfig(new MyNetworkConfig(configuration.serverIP, configuration.serverPort));
	}

    private string LoadFile()
    {
        return File.ReadAllText("./config.json");
    }

    // Update is called once per frame
    private void Update () {
		
	}
}
