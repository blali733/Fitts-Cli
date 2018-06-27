using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfoDisplay : MonoBehaviour
{
    public void SetupDisplay(string address, string port, int taskCount)
    {
        transform.Find("Address").GetComponent<Text>().text = string.Format("Connected to server @ {0}:{1}.", address, port);
        transform.Find("Count").GetComponent<Text>().text = string.Format("Server provides {0} test cases.", taskCount.ToString());
        transform.Find("SysInfo").GetComponent<Text>().text = string.Format("UID: {0}\nDM: {1}\nDN: {2}\nGDN: {3}",SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceModel, SystemInfo.deviceName, SystemInfo.graphicsDeviceName);
    }
}
