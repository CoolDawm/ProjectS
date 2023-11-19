using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class NegScreem : AuraV2
{
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, Range, LayerMask.GetMask(aim));
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<EffectsApplying>().ApplyDebuff(Effect, Power,Duration);
        }
    }
}
