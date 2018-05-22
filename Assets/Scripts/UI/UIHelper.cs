using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIHelper
    {
        public static GameObject CenterInParent(GameObject gameObject, GameObject parent)
        {
            gameObject.transform.parent = parent.transform;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            return gameObject;
        }
    }
}
