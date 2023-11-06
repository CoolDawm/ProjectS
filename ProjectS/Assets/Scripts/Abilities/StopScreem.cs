using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopScreem :Aura
{
    private bool isEmitting;

    private void Start()
    {
        Range = 15f;
        Duration = 10f;
        Effect = "movementSpeed";
    }


    public override void StartEmitting(GameObject emitter, String aim)
    {
        Collider[] colliders = Physics.OverlapSphere(emitter.transform.position, Range, LayerMask.GetMask(aim));
        foreach (Collider collider in colliders)
        {
            float tmpChar = collider.gameObject.GetComponent<Characteristics>().charDic[Effect];
            collider.gameObject.GetComponent<EffectsApplying>().ApplyBuff(Effect, tmpChar,Duration);
        }
    }
}
