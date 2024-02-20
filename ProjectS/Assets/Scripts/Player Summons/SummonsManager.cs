using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsManager : MonoBehaviour
{
    [SerializeField]
    private int maxSummonsAmount;
    private List<GameObject> _summons=new List<GameObject>();
    public int summonsAmount { get; private set; }

    private void Start()
    {
        summonsAmount = 0;
    }

    public void IncreaseMaxSummon()
    {
        maxSummonsAmount++;
    }
    public void AddSummon(GameObject summon)
    {
        if (summonsAmount + 1 > maxSummonsAmount)
        {
            RemoveSummon();
        }
        _summons.Add(summon);
        summonsAmount++;
    }
    public GameObject GetSummon()
    {
        return _summons[0];
    }

    private void RemoveSummon()
    {
        float minhealth=Mathf.Infinity;
        GameObject sumForDestroy=null;
        int i = -1;
        foreach (GameObject sum in _summons)
        {
            if (sum.GetComponent<HealthSystem>().currentHealth < minhealth)
            {
                i++;
                sumForDestroy = sum;
                minhealth = sum.GetComponent<HealthSystem>().currentHealth;
            }
        }
        _summons.RemoveAt(i);
        sumForDestroy.GetComponent<HealthSystem>().OnDeath.Invoke();
    }
}
