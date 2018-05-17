using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIPositioner
    {
        public static GameObject CenterInParent(GameObject gameObject)
        {
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            return gameObject;
        }
    }
}
