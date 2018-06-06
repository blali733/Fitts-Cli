using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionarieBackend : MonoBehaviour
{
    private Toggle _none;
    private Toggle _smaller5;
    private Toggle _smaller11;
    private Toggle _greater11;
    private void Awake()
    {
        _none = transform.Find("None").GetComponent<Toggle>();
        _smaller5 = transform.Find("less5").GetComponent<Toggle>();
        _smaller11 = transform.Find("less11").GetComponent<Toggle>();
        _greater11 = transform.Find("greater11").GetComponent<Toggle>();
    }
    public void SizeGroupNone()
    {
        if (_none.isOn)
        {
            _smaller5.isOn = false;
            _smaller11.isOn = false;
            _greater11.isOn = false;
        }
    }

    public void SizeGroupAny()
    {
        if (_smaller5.isOn || _smaller11.isOn || _greater11.isOn)
        {
            _none.isOn = false;
        }
        else
        {
            _none.isOn = true;
        }
    }
}
