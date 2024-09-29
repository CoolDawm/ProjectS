using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicsShow : MonoBehaviour
{
    private Characteristics _characteristics;
    private TextMeshProUGUI _text;
    private string _char;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        
    }

    private void OnEnable()
    {
        if (_characteristics == null)
        {
            _characteristics = GameObject.FindWithTag("Player").GetComponent<Characteristics>();
            _characteristics.OnUpdate += UpdateCharacteristicsText;
        }
        UpdateCharacteristicsText();
    }
    private void UpdateCharacteristicsText()
    {
        _char = "";
        foreach (var pair in _characteristics.charDic)
        {
            _char += pair.Key + " = " + pair.Value + "\n";
        }
        _text.text = _char;
    }
}
