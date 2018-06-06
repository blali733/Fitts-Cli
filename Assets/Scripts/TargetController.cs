using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    private GameObject item;
    GameObject _center;
    GameObject _frame;
    ConfigSingleton _settings;

    // Use this for initialization
    private void Awake () {
        _settings = ConfigSingleton.GetInstance();
        item = this.gameObject;
        _center = this.gameObject.transform.Find("Center").gameObject;
        _frame = this.gameObject.transform.Find("Frame").gameObject;
	}

    public void Construct(double scale, Vector2 position)
    {
        Construct(scale, position, Color.red);
    }

    public void Construct(double scale, Vector2 position, Color color)
    {
        item.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
        item.transform.position = new Vector3(position[0], position[1], -1);
        SetColor(color);
    }

    private void SetColor(Color color)
    {
        SpriteRenderer myRenderer = _center.GetComponent<SpriteRenderer>();
        myRenderer.color = color;
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
        SendMessageUpwards("Respawn", DateTime.Now);
        Destroy(item, 0);
    }
}
