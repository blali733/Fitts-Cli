using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharedMessages;
using UnityEngine;
using SharedTypes;
using UI;
using UnityEngine.Networking;

public class MainLoop : MonoBehaviour
{
    public GameObject ExperimentController;
    public GameObject WaitingScreen;
    public Canvas MainCanvas;
    private ConfigSingleton _config;
    private GameObject _waiting;

    private void Start()
    {
        _config = ConfigSingleton.GetInstance();
        // Temporary solution
        _waiting = UIHelper.CenterInParent(Instantiate(WaitingScreen), MainCanvas.gameObject);
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
    }

    public void GotConfig()
    {
        Destroy(_waiting);
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }
}
