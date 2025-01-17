﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    private GameObject _item;
    GameObject _center;
    private Color _color;
    private ConfigSingleton _config;

    // Use this for initialization
    private void Awake () {
        _item = this.gameObject;
        _center = this.gameObject.transform.Find("Center").gameObject;
        _config = ConfigSingleton.GetInstance();
	}

    public void Construct(double scale, Vector2 position)
    {
        Construct(scale, position, Color.red);
    }

    public void Construct(double scale, Vector2 position, Color color)
    {
        _item.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
        _item.transform.position = new Vector3(position[0], position[1], -1);
        SetColor(color);
    }

    private void SetColor(Color color)
    {
        SpriteRenderer myRenderer = _center.GetComponent<SpriteRenderer>();
        myRenderer.color = color;
        _color = color;
    }

    private void OnMouseDown()
    {
        Debug.Log(_config.ColorDiffBg(_color));
        SendMessageUpwards("Respawn", DateTime.Now);
        Destroy(_item, 0);
    }
}
