using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicsLoader : MonoBehaviour
{
    private Characteristics _characteristics;

    [SerializeField] private List<String> dirKeys;
    [SerializeField] private List<float> dirValues;
    // Start is called before the first frame update
    void Start()
    {
        _characteristics = gameObject.GetComponent<Characteristics>();
        for (int i = 0; i <dirKeys.Count; i++)
        {
            _characteristics.charDic.Add(dirKeys[i],dirValues[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
