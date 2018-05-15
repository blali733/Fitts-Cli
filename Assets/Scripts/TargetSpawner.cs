using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;

public class TargetSpawner : MonoBehaviour
{
    public GameObject TargetPrefab;
    private ConfigSingleton _config;
    private int _itemCounter = 0;
    private Vector2Int _range;
    private ColorMode _colorMode;

	// Use this for initialization
	private void Start ()
	{
	    _config = ConfigSingleton.GetInstance();
	}

    public void Setup(TestCase myTestCase)
    {
        _itemCounter = myTestCase.TargetsCount;
        _range = myTestCase.MinTargetScale > myTestCase.MaxTargetScale ? new Vector2Int(myTestCase.MaxTargetScale, myTestCase.MinTargetScale) : new Vector2Int(myTestCase.MinTargetScale, myTestCase.MaxTargetScale);
        _config.SetCameraProperties(myTestCase.DisplayMode);
        _colorMode = myTestCase.Color;
        GameObject target = Instantiate(TargetPrefab);
        target.transform.parent = this.transform;
        float size = _config.GetRandomSize(_range);
        Vector2 position = new Vector2(0,0);
        Color color = _config.GetColor(_colorMode);
        SendMessageUpwards("TargetAcquired", new TargetData(color, position[0], position[1], size));
        target.GetComponent<TargetController>().Construct(size, position, color);
    }

    public void Respawn()
    {
        if (_itemCounter <= 0) return;
        GameObject target = Instantiate(TargetPrefab);
        target.transform.parent = this.transform;
        float size = _config.GetRandomSize(_range);
        Vector2 position = _config.GetRandomPosition();
        Color color = _config.GetColor(_colorMode);
        SendMessageUpwards("TargetAcquired", new TargetData(color, position[0], position[1], size));
        target.GetComponent<TargetController>().Construct(size, position, color);
        _itemCounter--;
    }
}
