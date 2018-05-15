using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using SharedTypes;



public class ConfigSingleton : MonoBehaviour {
    //Singleton instance
    private static ConfigSingleton _instance;

    //Variables:
    private MyNetworkConfig _myNetworkConfig;
    private int _boardHeightPoints;
    private int _boardWidthPoints;
    private float _pointsPerUnit;
    private int _maxRange;
    private readonly System.Random _rng;
    private List<TestCase> _testCaseList;

    public float BoardWidth { get; private set; }

    public float BoardHeight { get; private set; }

    public bool IsSystemReady { get; private set; }

    public List<TestCase> GetTestCases()
    {
        return _testCaseList;
    }

    public Vector2 GetRandomPosition()
    {
        int pos = _rng.Next(_maxRange);
        int y = pos % _boardHeightPoints - _boardHeightPoints / 2;
        int x = pos / _boardHeightPoints - _boardWidthPoints / 2;
        return new Vector2(x/_pointsPerUnit, y / _pointsPerUnit);
    }

    public float GetRandomSize(Vector2Int range)
    {
        int size = _rng.Next(range[0] > 100 ? 100 : range[0], range[1] > 100 ? 100 : range[1]);
        return size / (float)100.0;
    }

    public Color GetColor(ColorMode colorMode)
    {
        switch (colorMode)
        {
            case ColorMode.StaticGreen:
                return Color.green;
            case ColorMode.StaticRed:
                return Color.red;
            case ColorMode.StaticBlue:
                return Color.blue;
            default:
                return Color.black;
        }
    }

    public void SetCameraProperties(DisplayMode mode)
    {
        switch (mode)
        {
            case DisplayMode.ConstantPixelSize:
            {
                Camera.main.GetComponent<Camera>().orthographicSize = (float)Screen.height / 100;
                BoardHeight = Camera.main.GetComponent<Camera>().orthographicSize * 2;
                _pointsPerUnit = 100;
                _boardHeightPoints = (int)((BoardHeight - 2) * _pointsPerUnit);
                BoardWidth = BoardHeight * Screen.width / Screen.height;
                _boardWidthPoints = (int)((BoardWidth - 2) * _pointsPerUnit);
                _maxRange = _boardHeightPoints * _boardWidthPoints;
                break;
            }
            case DisplayMode.ConstantUnitSize:
            {
                Camera.main.GetComponent<Camera>().orthographicSize = 10;
                BoardHeight = Camera.main.GetComponent<Camera>().orthographicSize * 2;
                _pointsPerUnit = 100;
                _boardHeightPoints = (int)((BoardHeight - 2) * _pointsPerUnit);
                BoardWidth = BoardHeight * Screen.width / Screen.height;
                _boardWidthPoints = (int)((BoardWidth - 2) * _pointsPerUnit);
                _maxRange = _boardHeightPoints * _boardWidthPoints;
                break;
            }
        }
    }

    private ConfigSingleton()
    {
        SetCameraProperties(DisplayMode.ConstantUnitSize);
        _rng = new System.Random();
        IsSystemReady = false;
        _testCaseList = new List<TestCase>();
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
