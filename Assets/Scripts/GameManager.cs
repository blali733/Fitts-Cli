using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;

public class GameManager : MonoBehaviour
{
    public List<TargetData> TargetDatas;
    public List<TestCase> TestCases;

	// Use this for initialization
	void Start () {
	    TargetDatas = new List<TargetData>();	
	}

    public void TargetAcquired(TargetData targetData)
    {
        // Implement recalculation of positions
        TargetDatas.Add(targetData);
    }

    public void TestCompleted()
    {

    }
}
