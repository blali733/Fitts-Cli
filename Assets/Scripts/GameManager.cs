using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;
using UI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject _canvas;
    public GameObject TestAnnouncer;
    private List<TargetData> _targetDatas;
    private List<TestCase> _testCases;
    private ConfigSingleton _config;

	// Use this for initialization
	void Start ()
	{
	    _canvas = GameObject.Find("Canvas");
	    GameObject window = Instantiate(TestAnnouncer);
	    window.transform.parent = _canvas.transform;
	    window = UIPositioner.CenterInParent(window);
	    window.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate {transform.root.GetComponent<ButtonBehaviour>().TestAnnouncer();});
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
