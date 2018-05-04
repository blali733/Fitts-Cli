using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;

	// Use this for initialization
	void Start ()
	{
	    GameObject target = Instantiate(targetPrefab);
        target.GetComponent<TargetController>().Construct(1.0, new Vector2(0,0));
	    GameObject target2 = Instantiate(targetPrefab);
	    target2.GetComponent<TargetController>().Construct(0.5, new Vector2(5, -4), Color.green);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
