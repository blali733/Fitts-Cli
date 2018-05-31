using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public Canvas Canvas;

    public void TestAnnouncer()
    {
        Destroy(Canvas.transform.Find("TestAnnouncer(Clone)").gameObject);
        BroadcastMessage("MessageClosed");
    }

    public void HideConfig()
    {
        BroadcastMessage("ConfigClosed");
    }
}
