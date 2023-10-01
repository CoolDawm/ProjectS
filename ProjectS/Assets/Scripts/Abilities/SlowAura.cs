using System;
using System.Collections;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class SlowAura : Aura
{
    private bool isEmitting;

    private void Start()
    {
        Range = 15f;
        Power = 3f;
        Duration = 15f;
        Effect = "Slow";
    }


    public override void StartEmitting(GameObject emitter,String aim)
    {
        StartCoroutine(EmittingCoroutine(emitter, aim));
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
        }
        yield return null;
    }
}

