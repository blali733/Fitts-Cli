﻿using System.Collections;
using System.Collections.Generic;
using FittsLibrary.Messages;
using UnityEngine;
using FittsLibrary;
using UI;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    public GameObject TestAnnouncer;
    public GameObject UserType;
    public GameObject UserList;
    public GameObject UserListWaiting;
    public GameObject Questionarie;

    private GameObject _canvas;
    private List<TargetData> _targetDatas;
    private List<List<TargetData>> _experimentResults;
    private List<TestCase> _testCases;
    private ConfigSingleton _config;
    private int _testCounter;
    private GameObject _userForm;
    private string _userCode;

	// Use this for initialization
	void Start ()
	{
	    _testCounter = 0;
	    _config = ConfigSingleton.GetInstance();
	    _targetDatas = new List<TargetData>();
	    _experimentResults = new List<List<TargetData>>();      // Caring about complexity is overrated.
	    _testCases = _config.GetTestCases();
        _canvas = GameObject.Find("Canvas");
	    GameObject window = UIHelper.CenterInParent(Instantiate(UserType), _canvas);
	    window.transform.Find("New").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
	        transform.root.GetComponent<ButtonBehaviour>().NewUser();
	    });
	    window.transform.Find("Returning").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
	        transform.root.GetComponent<ButtonBehaviour>().ReturningUser();
	    });
    }

    public void NewUserForm()
    {
        _userForm = UIHelper.CenterInParent(Instantiate(Questionarie), _canvas);
        _userForm.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(ValidateForm);
    }

    public void ValidateForm()
    {
        if (_userForm.transform.Find("UserName").Find("Text").gameObject.GetComponent<Text>().text != "")
        {
            // Save to DB
//            _userCode = _userForm.transform.Find("UserName").Find("Text").gameObject.GetComponent<Text>().text;
            StoredUser user = new StoredUser(_userForm.transform.Find("UserName").Find("Text").gameObject.GetComponent<Text>().text, Helpers.ParseQuestionarie(_userForm));
            MyNetworkManager.singleton.client.Send(MyMsgType.NewUserData, new StoredUserMessage(user));
            Destroy(_userForm);
        }
        else
        {
            // Do nothing
            return;
        }
    }

    public void GotUserCode(string code)
    {
        _userCode = code;
        SetupTestWindow();
    }

    public void SelectUser(string userCode)
    {
        Destroy(_canvas.transform.Find("UserList(Clone)").gameObject);
        _userCode = userCode;
        SetupTestWindow();
    }

    public void ReturningUserForm()
    {
        GameObject window = UIHelper.CenterInParent(Instantiate(UserListWaiting), _canvas);
        MyNetworkManager.singleton.client.Send(MyMsgType.DataRequest, new RequestMessage(RequestType.UserList));
    }

    public void GotReturningUsers(List<User> userList)
    {
        Destroy(_canvas.transform.Find("UserListWaiting(Clone)").gameObject);
        GameObject window = UIHelper.CenterInParent(Instantiate(UserList), _canvas);
        window.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            transform.root.GetComponent<ButtonBehaviour>().ReturnToMain();
        });
        foreach (var user in userList)
        {
            window.GetComponent<ListController>().Populate(user.Name, user.Code);
        }
    }

    public void ReturnToMainTgt()
    {
        GameObject window = UIHelper.CenterInParent(Instantiate(UserType), _canvas);
        window.transform.Find("New").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            transform.root.GetComponent<ButtonBehaviour>().NewUser();
        });
        window.transform.Find("Returning").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            transform.root.GetComponent<ButtonBehaviour>().ReturningUser();
        });
    }

    public void SetupTestWindow()
    {
        GameObject window = UIHelper.CenterInParent(Instantiate(TestAnnouncer), _canvas);
        window = UIHelper.AddTestDescription(window, _testCases[_testCounter].Name);
        window.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            transform.root.GetComponent<ButtonBehaviour>().TestAnnouncer();
        });
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
        _targetDatas = new List<TargetData>();
        if (_testCounter < _testCases.Count)
        {
            SetupTestWindow();
        }
        else
        {
            SendMessageUpwards("ExperimentCompleted", new RawTargetDatasMessage.Pack(_experimentResults, _userCode));
            Destroy(transform.gameObject);
        }
    }
}
