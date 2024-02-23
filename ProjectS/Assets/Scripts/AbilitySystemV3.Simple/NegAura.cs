using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class NegAura : AuraV2
{
    private bool isEmitting;
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        if (isEmitting)
        {
            
        }
        else
        {
           coroutineRunner.StartCoroutineFunction(EmittingCoroutine(user)); 
           abilityIsActive = true;
        }
    }

    IEnumerator EmittingCoroutine(GameObject emitter)
    {
        Debug.Log("Down");
        isEmitting = true;
        float timer = 0;
        while (isEmitting)
        {
            Collider[] colliders = Physics.OverlapSphere(emitter.transform.position, range, LayerMask.GetMask(aim));
            if (timer <= Duration)
            {
                foreach (Collider collider in colliders)
                {
                    collider.gameObject.GetComponent<EffectsApplying>().ApplyDebuffAura(Effect,Power);
                }
            }
            else
            {
                isEmitting = false;
            }
            timer+=Time.deltaTime;
            
            yield return null;
        }
        abilityIsActive = false;
    }
}
