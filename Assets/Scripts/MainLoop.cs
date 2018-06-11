﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharedMessages;
using UnityEngine;
using SharedTypes;
using UI;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainLoop : MonoBehaviour
{
    public GameObject ExperimentController;
    public GameObject WaitingScreen;
    public GameObject DataDiplayScreen;
    public GameObject ConnectionBroker;
    public Canvas MainCanvas;
    private ConfigSingleton _config;
    private GameObject _waiting;
    private GameObject _shadow;

    private void Start()
    {
        _config = ConfigSingleton.GetInstance();
        _waiting = UIHelper.CenterInParent(Instantiate(ConnectionBroker), MainCanvas.gameObject);
        _waiting.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(SetupConnection);
//        _waiting = UIHelper.CenterInParent(Instantiate(WaitingScreen), MainCanvas.gameObject);
    }

    public void SetupConnection()
    {
        _waiting.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "";
        string ip = _waiting.transform.Find("InputField").Find("Text").gameObject.GetComponent<Text>().text;
        string port = _waiting.transform.Find("InputField (1)").Find("Text").gameObject.GetComponent<Text>().text;
        if (ip == "" || port == "")
        {
            _waiting.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Missing IP or port!";
            return;
        }
        int porti;
        if (!int.TryParse(port, out porti))
        {
            _waiting.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Port is not a number!";
            return;
        }
        _config.SetMyNetworkConfig(new MyNetworkConfig(ip, port));
        GameObject.Find("Configurator").gameObject.GetComponent<NetworkConnectionConfigurator>().SetupConnection();
        _waiting.SetActive(false);
        _shadow = UIHelper.CenterInParent(Instantiate(WaitingScreen), MainCanvas.gameObject);
    }

    public void ConnectionFailed()
    {
        Destroy(_shadow);
        _waiting.SetActive(true);
        _waiting.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Connection Failed";
    }

    public void ExperimentCompleted(List<List<TargetData>> experimentTargetDatas)
    {
        List<List<TargetInfo>> experimentTargetInfos = new List<List<TargetInfo>>();
        foreach (var list in experimentTargetDatas)
        {
            List<TargetInfo> iterationTargetInfos = new List<TargetInfo>();
            foreach (var i in Enumerable.Range(1, list.Count - 1))
            {
                iterationTargetInfos.Add(new TargetInfo(list[i-1], list[i]));
            }
            experimentTargetInfos.Add(iterationTargetInfos);
        }
        MyNetworkManager.singleton.client.Send(MyMsgType.TargetDatas, new RawTargetDatasMessage(experimentTargetDatas));
        MyNetworkManager.singleton.client.Send(MyMsgType.TargetInfos, new TargetInfosMessage(experimentTargetInfos));
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }

    public void GotConfig()
    {
        Destroy(_waiting);
        Destroy(_shadow);
        _waiting = UIHelper.CenterInParent(Instantiate(DataDiplayScreen), MainCanvas.gameObject);
        _waiting.transform.GetComponent<StatusInfoDisplay>().SetupDisplay(_config.GetMyNetworkConfig().Address, _config.GetMyNetworkConfig().Port, _config.GetTestCases().Count);
        _waiting.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate { transform.root.GetComponent<ButtonBehaviour>().HideConfig(); });
    }

    public void ConfigClosed()
    {
        Destroy(_waiting);
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }
}
