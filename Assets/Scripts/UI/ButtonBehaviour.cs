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

    public void NewUser()
    {
        Destroy(Canvas.transform.Find("UserType(Clone)").gameObject);
        BroadcastMessage("NewUserForm");
    }

    public void ReturningUser()
    {
        Destroy(Canvas.transform.Find("UserType(Clone)").gameObject);
        BroadcastMessage("ReturningUserForm");
    }

    public void ReturnToMain()
    {
        Destroy(Canvas.transform.Find("UserList(Clone)").gameObject);
        BroadcastMessage("ReturnToMain");
    }

    public void HideConfig()
    {
        BroadcastMessage("ConfigClosed");
    }

    public void SelectPlayer(string name)
    {
        // TODO: Implement
    }
}
