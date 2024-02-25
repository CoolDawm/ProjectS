using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsApplying : MonoBehaviour
{
    public float timer = 1f;
    private Dictionary<string, float> charDic;
    private bool _buffAply=false;
    private bool _debuffAply=false;
    // Start is called before the first frame update
    void Start()
    {
        charDic = GetComponent<Characteristics>().charBuffBuffer;
    }
    IEnumerator DebuffAura(String effect, float power)
    {
        timer = 3;
        _debuffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar - power;
        while (_debuffAply)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 1f;
                charDic[effect]=tmpChar;
                _debuffAply = false;
            }
            yield return null;
        }
    }
    IEnumerator BuffAura(String effect, float power)
    {
        timer = 3;
        _buffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar + power;
        while (_buffAply)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 1f;
                charDic[effect]=tmpChar;
                _buffAply = false;
                
            }
            yield return null;
        }
    }
    public void ApplyDebuffAura(String effect, float power)
    {
        if (!_debuffAply)
        {
            StartCoroutine(DebuffAura( effect, power));
        }
    }
    public void ApplyBuffAura(String effect, float power)
    {
        if (!_buffAply)
        {
            StartCoroutine(BuffAura( effect, power));
        }
    }

    public void ApplyDebuff(String effect, float power,float duration)
    {
        StartCoroutine(Debuff( effect, power,duration));
    }
    public void ApplyBuff(String effect, float power, float duration)
    {
        StartCoroutine(Buff( effect, power,duration));
    }
    IEnumerator Buff(String effect, float power, float duration)
    {
        bool buffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar + power;
        while (buffAply)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                charDic[effect] = tmpChar;
                buffAply = false;
               
            }

            yield return null;
        }
    }
    IEnumerator Debuff(String effect, float power, float duration)
    {
        bool buffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar - power;
        while (buffAply)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                charDic[effect] = tmpChar;
                buffAply = false;
            }

            yield return null;
        }
    }
}
