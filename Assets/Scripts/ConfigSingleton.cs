using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MyNetworkConfig
{
    public string address;
    public string port;
    public MyNetworkConfig(string address, string port)
    {
        this.address = address;
        this.port = port;
    }
}

public class ConfigSingleton : MonoBehaviour {
    //Singleton instance
    private static ConfigSingleton instance;

    //Variables:
    private MyNetworkConfig _myNetworkConfig = new MyNetworkConfig();

    //Instance getter
    public static ConfigSingleton GetInstance()
    {
        if (instance == null)
            instance = new ConfigSingleton();
        return instance;
    }

    public MyNetworkConfig getMyNetworkConfig()
    {
        return this._myNetworkConfig;
    }

    public void setMyNetworkConfig(MyNetworkConfig myNetworkConfig)
    {
        this._myNetworkConfig = myNetworkConfig;
    }
}
