using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Colourful;
using UnityEngine;
using FittsLibrary;



public class ConfigSingleton
{
    //Singleton instance
    private static ConfigSingleton _instance;

    #region Variables

    private MyNetworkConfig _myNetworkConfig;
    private int _boardHeightPoints;
    private int _boardWidthPoints;
    private float _boardHeightRange;
    private float _boardWidthRange;
    private readonly float _pointsPerUnit;
    private float _pixelsPerUnit;
    private Vector2 _centerPixelPosition;
    private int _maxRange;
    private readonly System.Random _rng;
    private List<TestCase> _testCaseList;
    private LabColor _skyboxColor;

    public float BoardWidth { get; private set; }

    public float BoardHeight { get; private set; }

    public int DevId { get; set; }

    #endregion

    #region Getters and setters

    public List<TestCase> GetTestCases()
    {
        return _testCaseList;
    }

    public void SetTestCases(List<TestCase> cases)
    {
        _testCaseList = cases;
    }

public MyNetworkConfig GetMyNetworkConfig()
    {
        return this._myNetworkConfig;
    }

    public void SetMyNetworkConfig(MyNetworkConfig myNetworkConfig)
    {
        this._myNetworkConfig = myNetworkConfig;
    }
    #endregion

    #region Generators
    public Vector2 GetRandomPosition(DistanceMode mode, float radius, TargetData targetData)
    {
        switch (mode)
        {
            case DistanceMode.Random:
            {
                int pos = _rng.Next(_maxRange);
                int y = pos % _boardHeightPoints - _boardHeightPoints / 2;
                int x = pos / _boardHeightPoints - _boardWidthPoints / 2;
                return new Vector2(x / _pointsPerUnit, y / _pointsPerUnit);
            }
            case DistanceMode.EqualDistance:
            {
                List<Vector2> correctPointsList = new List<Vector2>();
                float prevX = targetData.XUnitPosition;
                float prevY = targetData.YUnitPosition;
                float constDeltaCompontent = radius * radius - prevX * prevX;
                float minX = prevX - radius;
                float maxX = prevX + radius;
                float y1, y2;
                foreach (var x in Helpers.FloatRange(minX>(-_boardWidthRange)?minX:(-_boardWidthRange), maxX < (_boardWidthRange) ? maxX : (_boardWidthRange),1/_pointsPerUnit))
                {
                    var variableDeltaComponent = 2 * prevX * x - x * x;
                    y1 = prevY + (float) Math.Sqrt(constDeltaCompontent + variableDeltaComponent);
                    y2 = prevY - (float) Math.Sqrt(constDeltaCompontent + variableDeltaComponent);
                    if (Math.Abs(y1) <= _boardHeightRange)
                    {
                        correctPointsList.Add(new Vector2(x, y1));
                    }
                    if (Math.Abs(y2) <= _boardHeightRange)
                    {
                        correctPointsList.Add(new Vector2(x, y2));
                    }
                }
                return correctPointsList[_rng.Next(correctPointsList.Count)];
            }
            default:
                return new Vector2(0, 0);
        }
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
    #endregion

    #region Camera related
    public void SetCameraProperties(DisplayMode mode)
    {
        switch (mode)
        {
            case DisplayMode.ConstantPixelSize:
            {
                Camera.main.GetComponent<Camera>().orthographicSize = (float)Screen.height / 100;
                _pixelsPerUnit = 100;
                _boardHeightRange = Camera.main.GetComponent<Camera>().orthographicSize;
                BoardHeight = _boardHeightRange * 2;
                _boardHeightPoints = (int)((BoardHeight - 2) * _pointsPerUnit);
                _boardWidthRange = _boardHeightRange * Screen.width / Screen.height - 1;
                _boardHeightRange--;
                BoardWidth = BoardHeight * Screen.width / Screen.height;
                _boardWidthPoints = (int)((BoardWidth - 2) * _pointsPerUnit);
                _maxRange = _boardHeightPoints * _boardWidthPoints;
                break;
            }
            case DisplayMode.ConstantUnitSize:
            {
                Camera.main.GetComponent<Camera>().orthographicSize = 10;
                _pixelsPerUnit = (float) Screen.height / 10;
                _boardHeightRange = Camera.main.GetComponent<Camera>().orthographicSize;
                BoardHeight = _boardHeightRange * 2;
                _boardHeightPoints = (int)((BoardHeight - 2) * _pointsPerUnit);
                BoardWidth = BoardHeight * Screen.width / Screen.height;
                _boardWidthRange = _boardHeightRange * Screen.width / Screen.height - 1;
                _boardHeightRange--;
                _boardWidthPoints = (int)((BoardWidth - 2) * _pointsPerUnit);
                _maxRange = _boardHeightPoints * _boardWidthPoints;
                break;
            }
        }
    }

    public TargetData RecalculateCoordinates(TargetData inputData)
    {
        inputData.Duration = inputData.DestroyTime - inputData.SpawnTime;
        inputData.XPixelPosition = (int)(_centerPixelPosition[0] + (_pixelsPerUnit * inputData.XUnitPosition));
        inputData.YPixelPosition = (int)(_centerPixelPosition[1] + (_pixelsPerUnit * inputData.YUnitPosition));
        inputData.PixelSize = (int) (_pixelsPerUnit * inputData.UnitSize);
        inputData.PixelOriented = true;
        return inputData;
    }
    #endregion

    public double ColorDiffBg(Color color)
    {
        LabColor temp = Helpers.Color2Lab(color);
        return Helpers.LabDiff(temp, _skyboxColor);
    }

    public double ColorDiffBg(LabColor color)
    {
        return Helpers.LabDiff(color, _skyboxColor);
    }

    private ConfigSingleton()
    {
        _pointsPerUnit = 100;
        _centerPixelPosition = new Vector2((float) Screen.width / 2, (float) Screen.height / 2);
        SetCameraProperties(DisplayMode.ConstantUnitSize);
        _rng = new System.Random();
        _testCaseList = new List<TestCase>();
        _skyboxColor = Helpers.Color2Lab(Camera.main.backgroundColor);
    }

    //Instance getter
    public static ConfigSingleton GetInstance()
    {
        return _instance ?? (_instance = new ConfigSingleton());
    }
}
