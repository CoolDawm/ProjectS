using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatTextManager : MonoBehaviour
{ 
    public GameObject floatingText;
    public void ShowFloatingNumbers(Transform _object,float damage)
    {
        var number = Instantiate(floatingText, _object.position, _object.rotation,transform);
        number.GetComponent<TextMesh>().text = damage.ToString();
    }
    public void ShowFloatingText(Transform _object,String text)
    {
        
        var _text = Instantiate(floatingText, _object.position, _object.rotation,transform);
        _text.GetComponent<TextMesh>().text = text;
    }
}
