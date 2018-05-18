using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;
using UI;
using UnityEngine.Experimental.UIElements;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    private GameObject _canvas;
    public GameObject TestAnnouncer;
    private List<TargetData> _targetDatas;
    private List<List<TargetData>> _experimentResults;
    private List<TestCase> _testCases;
    private ConfigSingleton _config;
    private int _testCounter;

	// Use this for initialization
	void Start ()
	{
	    _testCounter = 0;
	    _canvas = GameObject.Find("Canvas");
	    GameObject window = UIPositioner.CenterInParent(Instantiate(TestAnnouncer), _canvas);
	    window.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate {transform.root.GetComponent<ButtonBehaviour>().TestAnnouncer();});
        _config = ConfigSingleton.GetInstance();
	    _targetDatas = new List<TargetData>();
	    _experimentResults = new List<List<TargetData>>();      // Caring about complexity is overrated.
	    _testCases = _config.GetTestCases();
	}

    public void MessageClosed()
    {
        transform.GetChild(0).GetComponent<TargetSpawner>().Setup(_testCases[_testCounter]);
        _testCounter++;
    }

    public void TargetAcquired(TargetData targetData)
    {
        _targetDatas.Add(_config.RecalculateCoordinates(targetData));
    }

    public void TestCompleted()
    {
        _experimentResults.Add(_targetDatas);
        if (_testCounter < _testCases.Count)
        {
            GameObject window = UIPositioner.CenterInParent(Instantiate(TestAnnouncer), _canvas);
            window.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                transform.root.GetComponent<ButtonBehaviour>().TestAnnouncer();
            });
        }
        else
        {
            SendMessageUpwards("ExperimentCompleted", _experimentResults);
        }
    }
}
