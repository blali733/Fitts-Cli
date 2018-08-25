using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FittsLibrary;

public class TargetSpawner : MonoBehaviour
{
    public GameObject FramedTargetPrefab;
    public GameObject FramelessTargetPrefab;
    private GameObject TargetPrefab;
    private ConfigSingleton _config;
    private int _itemCounter = 0;
    private int _colorRangeCategory;
    private Vector2Int _range;
    private ColorMode _colorMode;
    private TestCase _testCase;
    private TargetData _targetData;

	// Use this for initialization
	private void Awake ()
	{
	    _config = ConfigSingleton.GetInstance();
	}

    public void Setup(TestCase myTestCase)
    {
        _testCase = myTestCase;
        TargetPrefab = (_testCase.TargetMode == TargetMode.Framed ? FramedTargetPrefab : FramelessTargetPrefab);
        _itemCounter = _testCase.TargetsCount;
        _colorRangeCategory = 0;
        _range = _testCase.MinTargetScale > _testCase.MaxTargetScale ? new Vector2Int(_testCase.MaxTargetScale, _testCase.MinTargetScale) : new Vector2Int(_testCase.MinTargetScale, _testCase.MaxTargetScale);
        _config.SetCameraProperties(_testCase.DisplayMode);
        _colorMode = _testCase.Color;
        if (_colorMode == ColorMode.Space)
        {
            _colorRangeCategory = _config.ColorRanges.Count - 1;
        }
        GameObject target = Instantiate(TargetPrefab);
        target.transform.parent = this.transform;
        float size = _config.GetRandomSize(_range);
        Vector2 position = new Vector2(0,0);
        Color color = _config.GetColor(_colorMode, _colorRangeCategory);
        _targetData = new TargetData(color, position[0], position[1], size, DateTime.Now, _config.ColorDiffBg(color));
        target.GetComponent<TargetController>().Construct(size, position, color);
    }

    public void Respawn(DateTime now)
    {
        if (_itemCounter <= 0)
        {
            if (_colorRangeCategory == 0)
            {
                _targetData.DestroyTime = now;
                SendMessageUpwards("TargetAcquired", _targetData);
                SendMessageUpwards("TestCompleted");
                return;
            }
            _itemCounter = _testCase.TargetsCount;
            _colorRangeCategory--;
        }
        _targetData.DestroyTime = now;
        SendMessageUpwards("TargetAcquired", _targetData);
        GameObject target = Instantiate(TargetPrefab);
        target.transform.parent = this.transform;
        float size = _config.GetRandomSize(_range);
        Vector2 position = _config.GetRandomPosition(_testCase.DistanceMode, _testCase.Radius, _targetData, size);
        Color color = _config.GetColor(_colorMode, _colorRangeCategory);
        _targetData = new TargetData(color, position[0], position[1], size, System.DateTime.Now, _config.ColorDiffBg(color));
        target.GetComponent<TargetController>().Construct(size, position, color);
        _itemCounter--;
    }
}
