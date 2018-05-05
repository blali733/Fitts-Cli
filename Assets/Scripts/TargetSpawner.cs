using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    private ConfigSingleton config;

	// Use this for initialization
	void Start ()
	{
        config = ConfigSingleton.GetInstance();
	    GameObject target = Instantiate(targetPrefab);
	    target.transform.parent = this.transform;
        target.GetComponent<TargetController>().Construct(config.GetRandomSize(), config.GetRandomPosition());
    }

    public void Respawn()
    {
        GameObject target = Instantiate(targetPrefab);
        target.transform.parent = this.transform;
        target.GetComponent<TargetController>().Construct(config.GetRandomSize(), config.GetRandomPosition());
    }
}
