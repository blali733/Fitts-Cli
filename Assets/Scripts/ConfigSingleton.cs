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

    //Variables:
    private MyNetworkConfig _myNetworkConfig = new MyNetworkConfig();
    private readonly float _boardHeight;
    private readonly float _boardWidth;
    private readonly int _boardHeightPoints;
    private readonly int _boardWidthPoints;
    private readonly float _pointsPerUnit;
    private readonly int _maxRange;
    private readonly System.Random _rng;

    public float BoardWidth
    {
        get { return _boardWidth; }
    }

    public float BoardHeight
    {
        get { return _boardHeight; }
    }

    public Vector2 GetRandomPosition()
    {
        int pos = _rng.Next(_maxRange);
        int y = pos % _boardHeightPoints - _boardHeightPoints / 2;
        int x = pos / _boardHeightPoints - _boardWidthPoints / 2;
        return new Vector2(x/_pointsPerUnit, y / _pointsPerUnit);
    }

    public float GetRandomSize()
    {
        int size = _rng.Next(25, 100);
        return size / (float)100.0;
    }

    private ConfigSingleton()
    {
        Camera.main.GetComponent<Camera>().orthographicSize = (float)Screen.height / 100;
        _boardHeight = Camera.main.GetComponent<Camera>().orthographicSize*2;
        _pointsPerUnit = 100;
        _boardHeightPoints = (int) ((_boardHeight - 2) * _pointsPerUnit);
        _boardWidth = _boardHeight * Screen.width / Screen.height;
        _boardWidthPoints = (int) ((_boardWidth - 2) * _pointsPerUnit);
        _maxRange = _boardHeightPoints*_boardWidthPoints;
        _rng = new System.Random();
    }

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
