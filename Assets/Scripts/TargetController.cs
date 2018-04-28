using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    GameObject item;
    GameObject center;
    GameObject frame;
    ConfigSingleton settings;

    // Use this for initialization
    void Start () {
        settings = ConfigSingleton.GetInstance();
        item = this.gameObject;
        center = this.gameObject.transform.Find("Center").gameObject;
        frame = this.gameObject.transform.Find("Frame").gameObject;
        Construct();
	}

    void Construct(float scale, Vector2 position)
    {
        // I really LOVE C# for this shit...
        Construct(scale, position, Color.red);
    }

    void Construct(float scale, Vector2 position, Color color)
    {

    }

    void SetVisible(bool desiredState)
    {
        item.SetActive(desiredState);
    }

    void SetColor()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
