using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class ConfigContent
{
    public string serverIP;
    public string serverPort;
}

public class ConfigLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string config = LoadFile();
        ConfigContent configuration = new ConfigContent();
        configuration = JsonUtility.FromJson<ConfigContent>(config);
        ConfigSingleton configInstance = ConfigSingleton.GetInstance();
        configInstance.setMyNetworkConfig(new MyNetworkConfig(configuration.serverIP, configuration.serverPort));
	}

    string LoadFile()
    {
        return File.ReadAllText("./config.json");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
