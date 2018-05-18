using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedTypes;

public class MainLoop : MonoBehaviour
{
    public GameObject ExperimentController;
    private ConfigSingleton _config;

    private void Start()
    {
        _config = ConfigSingleton.GetInstance();
        // Temporary solution
        GameObject experiment = Instantiate(ExperimentController);
        experiment.transform.parent = this.transform;
    }

    public void ExperimentCompleted(List<List<TargetData>> experimentTargetDatas)
    {
        // Handle sending to databases here.
    }
}
