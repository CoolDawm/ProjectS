using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatTextManager : MonoBehaviour
{ 
    public GameObject floatingText;
    public void ShowFloatingNumbers(Transform _object,float damage,Color color)
    {
        var number = Instantiate(floatingText, _object.position, _object.rotation,transform);
        number.GetComponent<TextMesh>().text = damage.ToString();
        number.GetComponent<TextMesh>().color = color;
    }
    public void ShowFloatingText(Transform _object,String text,Color color)
    {
        
        var _text = Instantiate(floatingText, _object.position, _object.rotation,transform);
        _text.GetComponent<TextMesh>().text = text;
        _text.GetComponent<TextMesh>().color = color;
        
    }
}
