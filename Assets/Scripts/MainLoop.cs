using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : MonoBehaviour
{
    public GameObject ExperimentController;

	void Start () {
		// Temporary solution
	    GameObject experiment = Instantiate(ExperimentController);
	    experiment.transform.parent = this.transform;
	}
}
