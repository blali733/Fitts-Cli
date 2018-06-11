using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHelper
    {
        public static GameObject CenterInParent(GameObject gameObject, GameObject parent)
        {
            gameObject.transform.SetParent(parent.transform, false);
//            gameObject.transform.parent = parent.transform;
//            gameObject.transform.localScale = Vector3.one;
//            gameObject.transform.localPosition = Vector3.zero;
            return gameObject;
        }

        public static GameObject AddTestDescription(GameObject gameObject, string desc)
        {
            gameObject.transform.Find("Text").GetComponent<Text>().text = string.Format("Prepare for: {0}", desc);
            return gameObject;
        }
    }
}
