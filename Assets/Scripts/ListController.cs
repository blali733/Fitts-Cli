﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour
{
    public GameObject ListElmentPrefab;
    private GameObject GridHandler;
    private GameObject Handle;

    private void Awake()
    {
        GridHandler = transform.Find("ListPanel").Find("GridHolder").gameObject;
        Handle = GameObject.Find("Configurator");
    }

    public void Populate(string message, string userName)
    {
        GameObject item = Instantiate(ListElmentPrefab);
        item.transform.SetParent(GridHandler.transform);
        item.transform.Find("Text").GetComponent<Text>().text = message;
        item.GetComponent<Button>().onClick.AddListener(delegate {
            Handle.GetComponent<ButtonBehaviour>().SelectPlayer(userName);
        });
    }
}
