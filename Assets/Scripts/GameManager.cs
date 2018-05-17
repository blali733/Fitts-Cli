using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;

public class GameManager : MonoBehaviour
{
    private List<TargetData> _targetDatas;
    private List<TestCase> _testCases;
    private ConfigSingleton _config;

	// Use this for initialization
	void Start ()
	{
        _config = ConfigSingleton.GetInstance();
	    _targetDatas = new List<TargetData>();
	    _testCases = _config.GetTestCases();
        transform.GetChild(0).GetComponent<TargetSpawner>().Setup(_testCases[0]);
	}

    public void TargetAcquired(TargetData targetData)
    {
        // Implement recalculation of positions
        _targetDatas.Add(targetData);
    }

    public void TestCompleted()
    {

    }
}
