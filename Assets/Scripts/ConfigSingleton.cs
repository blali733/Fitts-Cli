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
    private static ConfigSingleton _instance;
    private readonly float _boardHeight;
    private readonly float _boardWidth;

    public float BoardWidth
    {
        get { return _boardWidth; }
    }

    public float BoardHeight
    {
        get { return _boardHeight; }
    }

    private ConfigSingleton()
    {
        _boardHeight = Camera.main.GetComponent<Camera>().orthographicSize;
        _boardWidth = _boardHeight * Screen.width / Screen.height;
    }

    //Variables:
    private MyNetworkConfig _myNetworkConfig = new MyNetworkConfig();

    //Instance getter
    public static ConfigSingleton GetInstance()
    {
        if (_instance == null)
            _instance = new ConfigSingleton();
        return _instance;
    }

    public MyNetworkConfig GetMyNetworkConfig()
    {
        return this._myNetworkConfig;
    }

    public void SetMyNetworkConfig(MyNetworkConfig myNetworkConfig)
    {
        this._myNetworkConfig = myNetworkConfig;
    }
}
