﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FittsLibrary.Messages;
using UnityEngine;
using FittsLibrary;
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
    private DeviceClass _deviceClass;

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
        _deviceClass = (DeviceClass) _waiting.transform.Find("Dropdown").gameObject.GetComponent<Dropdown>().value;
    }

    public void ConnectionFailed()
    {
        Destroy(_shadow);
        _waiting.SetActive(true);
        _waiting.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Connection Failed";
    }

    public void ExperimentCompleted(RawTargetDatasMessage.Pack data)
    {
        List<List<TargetInfo>> experimentTargetInfos = new List<List<TargetInfo>>();
        foreach (var list in data.TargetDatas)
        {
            List<TargetInfo> iterationTargetInfos = new List<TargetInfo>();
            foreach (var i in Enumerable.Range(1, list.Count - 1))
            {
                iterationTargetInfos.Add(new TargetInfo(list[i-1], list[i], _config.ColorDiffBg(list[i].Color)));
            }
            experimentTargetInfos.Add(iterationTargetInfos);
        }
//        MyNetworkManager.singleton.client.Send(MyMsgType.TargetDatas, new RawTargetDatasMessage(data.TargetDatas, data.User));
//        MyNetworkManager.singleton.client.Send(MyMsgType.TargetInfos, new TargetInfosMessage(experimentTargetInfos, data.User, _config.DeviceString));
        var paths = GetFileNames(data.User);
        Helpers.SerializeObject(new RawTargetDatasMessage(data.TargetDatas, data.User), paths.Item1);
        Helpers.SerializeObject(new TargetInfosMessage(experimentTargetInfos, data.User, _config.DeviceString), paths.Item2);
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }

    private Tuple<string, string> GetFileNames(string userName)
    {
        var path = GetFilePath() + "/" + GetCurrentDateTime() + "_" + userName + "-";
        return new Tuple<string, string>(path + "rtd.dat", path + "ti.dat");
    }

    private string GetCurrentDateTime()
    {
        return DateTime.Now.Year+"."+DateTime.Now.Month+"."+DateTime.Now.Day+" "+DateTime.Now.Hour+"_"+DateTime.Now.Minute+"_"+DateTime.Now.Second;
    }
    
    private string GetFilePath()
    {
        string path;
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath;
        }
        else
        {
            path = ".";
        }
        path += "/results";
        Directory.CreateDirectory(path);
        return path;
    }

    public void GotConfig()
    {
        Destroy(_waiting);
        Destroy(_shadow);
        _waiting = UIHelper.CenterInParent(Instantiate(DataDiplayScreen), MainCanvas.gameObject);
        _waiting.transform.GetComponent<StatusInfoDisplay>().SetupDisplay(_config.GetMyNetworkConfig().Address, _config.GetMyNetworkConfig().Port, _config.GetTestCases().Count);
        _waiting.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate { transform.root.GetComponent<ButtonBehaviour>().HideConfig(); });
        _waiting.transform.Find("Button").gameObject.SetActive(false);
        var dev = new DeviceDataMessage(new DeviceIdentification(_deviceClass));
        _config.DeviceString = dev.DeviceIdentification.DevId;
        MyNetworkManager.singleton.client.Send(MyMsgType.DeviceData, dev);
    }

    public void GotColorSpace()
    {
        _waiting.transform.Find("Button").gameObject.SetActive(true);
        MyNetworkManager.IsConnected = true;
    }

    public void GotDevID()
    {
        var tc = _config.GetTestCases();
        foreach (var test in tc)
        {
            if (test.Color == ColorMode.Space)
            {
                MyNetworkManager.singleton.client.Send(MyMsgType.DataRequest, new RequestMessage(RequestType.ColorRanges));
                return;
            }
        }
        GotColorSpace();
    }

    public void ConfigClosed()
    {
        Destroy(_waiting);
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }
}
