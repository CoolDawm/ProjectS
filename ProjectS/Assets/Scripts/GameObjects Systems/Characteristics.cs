using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    private bool buffAply=false;
    private bool debuffAply=false;
    public float timer = 1f;
    public Dictionary<string, float> charDic = new Dictionary<string, float>();
    
    IEnumerator DebuffAura(String effect, float power)
    {
        debuffAply = true;
        float tmpChar = charDic[effect];
        charDic[effect] = tmpChar - power;
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
        charDic[effect] = tmpChar + power;
        while (buffAply)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 1f;
                charDic[effect]=tmpChar;
                buffAply = false;
                
            }
            yield return null;
        }
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
