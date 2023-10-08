using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    private bool buffAply=false;
    private bool debuffAply=false;
    public float timer = 1f;
    public GameObject _floatingText;
    private FloatTextManager _floatingTextManager;

    private void Start()
    {
        _floatingTextManager = GameObject.FindGameObjectWithTag("FloatingTextManager").GetComponent<FloatTextManager>();
    }

    public Dictionary<string, float> charDic = new Dictionary<string, float>()
    {
        {"maxSpeed",10f},
        {"damage",30f},
        {"stamina",100f},
        {"hp",100f},
        {"mana",100f},
        {"meleeRange",2f},
        {"projectileLife",5f},
        {"movementSpeed",5f}
    };
    
    IEnumerator DebuffAura(String effect, float power)
    {
        debuffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar - power;
        _floatingTextManager.ShowFloatingText(gameObject,"-"+effect);
        while (debuffAply)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 1f;
                charDic[effect]=tmpChar;
                debuffAply = false;
            }
            yield return null;
        }
    }
    IEnumerator BuffAura(String effect, float power)
    {
        buffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar +power;
        _floatingTextManager.ShowFloatingText(gameObject,"-"+effect);
        while (debuffAply)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 1f;
                charDic[effect]=tmpChar;
                debuffAply = false;
            }
        }
        yield return null;
    }
    public void ApplyDebuffAura(String effect, float power)
    {
        if (!debuffAply)
        {
            StartCoroutine(DebuffAura( effect, power));
        }
    }
    public void ApplyBuffAura(String effect, float power)
    {
        if (!buffAply)
        {
            StartCoroutine(BuffAura( effect, power));
        }
    }
}
