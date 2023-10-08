using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTextManager : MonoBehaviour
{
    public GameObject _floatingText;
    public void ShowFloatingNumbers(GameObject _object,float damage)
    {
        
        var number = Instantiate(_floatingText, _object.transform.position, _object.transform.rotation,transform);
        number.GetComponent<TextMesh>().text = damage.ToString();
    }
    public void ShowFloatingText(GameObject _object,String text)
    {
        
        var _text = Instantiate(_floatingText, _object.transform.position, _object.transform.rotation,transform);
        _text.GetComponent<TextMesh>().text = text;
    }
}
