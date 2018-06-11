using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour
{
    public GameObject ListElmentPrefab;
    private GameObject _gridHandler;
    public GameObject Handle;

    private void Awake()
    {
        _gridHandler = transform.Find("ListPanel").Find("GridHolder").gameObject;
        Handle = GameObject.Find("MainLoopController");
    }

    public void Populate(string message, string userName)
    {
        GameObject item = Instantiate(ListElmentPrefab);
        item.transform.SetParent(_gridHandler.transform, false);
        item.transform.Find("Text").GetComponent<Text>().text = message;
        item.GetComponent<Button>().onClick.AddListener(delegate {
            Handle.GetComponent<ButtonBehaviour>().SelectPlayer(userName);
        });
    }
}
