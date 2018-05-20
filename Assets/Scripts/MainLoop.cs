using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;
using UI;

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
        _waiting = UIPositioner.CenterInParent(Instantiate(WaitingScreen), MainCanvas.gameObject);
    }

    public void ExperimentCompleted(List<List<TargetData>> experimentTargetDatas)
    {
        // Handle sending to databases here.
    }

    public void GotConfig()
    {
        Destroy(_waiting);
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }
}
