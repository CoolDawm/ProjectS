using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicsLoader : MonoBehaviour
{
    private Characteristics _characteristics;
    [SerializeField] private List<String> dirKeys;
    [SerializeField] private List<float> dirValues;

    public float modifier = 1;
    // Start is called before the first frame update
    void Start()
    {
        _characteristics = gameObject.GetComponent<Characteristics>();
        modifier = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>().modifier;
        if (gameObject.CompareTag("Enemy"))
        {
            for (int i = 0; i <dirKeys.Count; i++)
            {
                _characteristics.charDic.Add(dirKeys[i],dirValues[i]*modifier);
            }
        }
        else
        {
            for (int i = 0; i <dirKeys.Count; i++)
            {
                _characteristics.charDic.Add(dirKeys[i],dirValues[i]);
            }
        }
        
    }
}
