using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Colourful;
using Colourful.Conversion;
using UnityEngine;
using FittsLibrary;
using Random = UnityEngine.Random;


public class ConfigSingleton
{
    //Singleton instance
    private static ConfigSingleton _instance;

    #region Variables

    private static int _targetRadius = 4;
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

    public List<ColorRange> ColorRanges { get; set; }

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

    private Vector2 GetIDRangeFromGroup(int group)
    {
        switch (group)
        {
            case 0:
                return new Vector2(0, (float)1.5);
            case 1:
                return new Vector2((float)1.5, 3);
            case 2:
                return new Vector2(3, (float)4.7);
        }
        // Should not happen:
        return new Vector2(0, 7);
    }

    private float GetRandomFloatFromRange(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    private Vector2 GetRandomPositionAtRange(float radius, float prevX, float prevY)
    {
        List<Vector2> correctPointsList = new List<Vector2>();
        float constDeltaCompontent = radius * radius - prevX * prevX;
        float minX = prevX - radius;
        float maxX = prevX + radius;
        foreach (var x in Helpers.FloatRange(minX>(-_boardWidthRange)?minX:(-_boardWidthRange), maxX < (_boardWidthRange) ? maxX : (_boardWidthRange),1/_pointsPerUnit))
        {
            var variableDeltaComponent = 2 * prevX * x - x * x;
            float y1 = prevY + (float) Math.Sqrt(constDeltaCompontent + variableDeltaComponent);
            float y2 = prevY - (float) Math.Sqrt(constDeltaCompontent + variableDeltaComponent);
            if (Math.Abs(y1) <= _boardHeightRange)
            {
                correctPointsList.Add(new Vector2(x, y1));
            }
            if (Math.Abs(y2) <= _boardHeightRange)
            {
                correctPointsList.Add(new Vector2(x, y2));
            }
        }

        if (correctPointsList.Count == 0)
        {
            Debug.Log("Critical error!");
        }
        return correctPointsList[_rng.Next(correctPointsList.Count)];
    }

    private float InitializeRanges(float targetScale)
    {
        float targetSize = 2 * _targetRadius * targetScale;
        _boardHeightPoints = (int)((BoardHeight - targetSize) * _pointsPerUnit);
        _boardHeightRange = (BoardHeight - targetSize) / 2;
        _boardWidthPoints = (int)((BoardWidth - targetSize) * _pointsPerUnit);
        _boardWidthRange = (BoardWidth - targetSize) / 2;
        _maxRange = _boardHeightPoints * _boardWidthPoints;
        return targetSize;
    }

    private List<Vector2> CalculateActiveRegionBorders(float scale)
    {
        float targetSize = _targetRadius * scale/100;
        float heightBorder = BoardHeight / 2 - targetSize;
        float widthBorder = BoardWidth / 2 - targetSize;
        return new List<Vector2>{
            new Vector2(widthBorder, heightBorder),
            new Vector2(-widthBorder, heightBorder),
            new Vector2(-widthBorder, -heightBorder),
            new Vector2(widthBorder, -heightBorder)
        };
    }

    private int MaxTargetScale(float maxScale, float prevX, float prevY, float ID)
    {
        List<Vector2> borders = CalculateActiveRegionBorders(maxScale);
        double maxDistance = 0;
        foreach (var point in borders)
        {
            double dist = Math.Sqrt(Math.Pow(prevX - point.x, 2) + Math.Pow(prevY - point.y, 2));
            if (dist > maxDistance)
                maxDistance = dist;
        }
        double maxWidth = maxDistance / Math.Pow(2, ID - 1);
        maxWidth /= 2;
        return (int) (maxWidth / _targetRadius * 100);
    }
    
    //TODO: Refactor this!!!
    public Tuple<Vector2, float> GetRandomPosition(DistanceMode mode, float predefinedRadius, TargetData targetData, int group, Vector2Int scaleRange)
    {
        switch (mode)
        {
            case DistanceMode.Random:
            {
                float scale = GetRandomScale(scaleRange);
                InitializeRanges(scale);
                int pos = _rng.Next(_maxRange);
                int y = pos % _boardHeightPoints - _boardHeightPoints / 2;
                int x = pos / _boardHeightPoints - _boardWidthPoints / 2;
                return new Tuple<Vector2, float>(new Vector2(x / _pointsPerUnit, y / _pointsPerUnit), scale);
            }
            case DistanceMode.EqualDistance:
            {
                float scale = GetRandomScale(scaleRange);
                InitializeRanges(scale);
                return new Tuple<Vector2, float>(GetRandomPositionAtRange(predefinedRadius, targetData.XUnitPosition, targetData.YUnitPosition), scale);
            }
            case DistanceMode.LinRegOptimised:
            {
                // Generate desired Difficulty Index:
                Vector2 range = GetIDRangeFromGroup(group);
                float desiredID = GetRandomFloatFromRange(range);
                // Calculate bounds of target width:
                int upperBound = MaxTargetScale(scaleRange.y, targetData.XUnitPosition, targetData.YUnitPosition,
                    desiredID);
                Vector2Int adjustedScaleRange =
                    new Vector2Int(scaleRange.x, (scaleRange.y > upperBound ? upperBound : scaleRange.y));
                // Generate scale and target size:
                float scale = GetRandomScale(adjustedScaleRange);
                float targetSize = InitializeRanges(scale);
                // Find all posible target locations and pick one:
                float radius = (float) (targetSize * Math.Pow(2, desiredID - 1));
                return new Tuple<Vector2, float>(
                    GetRandomPositionAtRange(radius, targetData.XUnitPosition, targetData.YUnitPosition), scale);
            }
            default:
                return new Tuple<Vector2, float>(new Vector2(0, 0), 0.0f);
        }
    }

    public float GetRandomScale(Vector2Int range)
    {
        int size = _rng.Next(range[0] > 100 ? 100 : range[0], range[1] > 100 ? 100 : range[1]);
        return size / (float)100.0;
    }

    public Color GetColor(ColorMode colorMode, int labCategory)
    {
        switch (colorMode)
        {
            case ColorMode.StaticGreen:
                return Color.green;
            case ColorMode.StaticRed:
                return Color.red;
            case ColorMode.StaticBlue:
                return Color.blue;
            case ColorMode.Space:
                Debug.Log(ColorRanges[labCategory].Distance);
                return ColorRanges[labCategory].Colors[_rng.Next(ColorRanges[labCategory].Colors.Count)];
            default:
                return Color.black;
        }
    }
    #endregion

    #region Camera related
    public void SetCameraProperties(DisplayMode mode)
    {
        if (mode == DisplayMode.ConstantPixelSize)
        {
            Camera.main.GetComponent<Camera>().orthographicSize = (float) Screen.height / 100;
            _pixelsPerUnit = 100;
        }
        else
        {
            Camera.main.GetComponent<Camera>().orthographicSize = 10;
            _pixelsPerUnit = (float)Screen.height / 10;
        }
        BoardHeight = Camera.main.GetComponent<Camera>().orthographicSize * 2;
        BoardWidth = BoardHeight * Screen.width / Screen.height;
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
