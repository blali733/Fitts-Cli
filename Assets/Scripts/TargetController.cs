using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    GameObject item;
    GameObject center;
    GameObject frame;
    ConfigSingleton settings;

    // Use this for initialization
    private void Awake () {
        settings = ConfigSingleton.GetInstance();
        item = this.gameObject;
        center = this.gameObject.transform.Find("Center").gameObject;
        frame = this.gameObject.transform.Find("Frame").gameObject;
	}

    public void Construct(double scale, Vector2 position)
    {
        // I really LOVE C# for this shit...
        Construct(scale, position, Color.red);
    }

    public void Construct(double scale, Vector2 position, Color color)
    {
        item.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
        item.transform.position = new Vector3(position[0], position[1], 0);
        SetColor(color);
    }

    private void SetVisible(bool desiredState)
    {
        item.SetActive(desiredState);
    }

    private void SetColor(Color color)
    {
        SpriteRenderer renderer = center.GetComponent<SpriteRenderer>();
        renderer.color = color;

    }

    // Update is called once per frame
    private void Update () {
		
	}
}
