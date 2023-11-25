using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicsShow : MonoBehaviour
{
    private Characteristics _characteristics;
    private Text _text;
    private string _char;
    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        _characteristics = GameObject.FindWithTag("Player").GetComponent<Characteristics>();
        _char = "";
        foreach (var pair in _characteristics.charDic)
        {
            _char += pair.Key + " = " + pair.Value + "\n";
        }
    }
    
    void FixedUpdate()
    {
        _text.text = _char;
    }
}
