using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageUpAura :  Aura
{
    private bool isEmitting;

    private void Start()
    {
        Range = 15f;
        Power = 5f;
        Duration = 15f;
        Effect = "damage";
    }


    public override void StartEmitting(GameObject emitter,String aim)
    {
        if (isEmitting)
        {
            
        }
        else
        {
            StartCoroutine(EmittingCoroutine(emitter, aim));
        }
    }
 
    IEnumerator EmittingCoroutine(GameObject emitter,String aim)
    {
        isEmitting = true;
        float timer = 0f;
        while (isEmitting)
        {
            Collider[] colliders = Physics.OverlapSphere(emitter.transform.position, Range, LayerMask.GetMask(aim));
            if (timer <= Duration)
            {
                emitter.GetComponent<EffectsApplying>().ApplyBuffAura(Effect,Power);
                foreach (Collider collider in colliders)
                {
                    collider.gameObject.GetComponent<EffectsApplying>().ApplyBuffAura(Effect,Power);
                }
            }
            else
            {
                isEmitting = false;
            }
            timer+=Time.deltaTime;
            yield return null;
        }
    }
}
