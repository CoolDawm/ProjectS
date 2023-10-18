using System;
using System.Collections;
using UnityEngine;
public class SlowAura : Aura
{
    private bool isEmitting;

    private void Start()
    {
        Range = 15f;
        Power = 2f;
        Duration = 15f;
        Effect = "movementSpeed";
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
        float timer = 0;
        while (isEmitting)
        {
            Collider[] colliders = Physics.OverlapSphere(emitter.transform.position, Range, LayerMask.GetMask(aim));
            if (timer <= Duration)
            {
                foreach (Collider collider in colliders)
                {
                    collider.gameObject.GetComponent<Characteristics>().ApplyDebuffAura(Effect,Power);
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

