using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PosScreem : AuraV2
{
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        user.GetComponent<EffectsApplying>().ApplyBuff(Effect, Power,Duration);
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range, LayerMask.GetMask(aim));
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<EffectsApplying>().ApplyBuff(Effect, Power,Duration);
        }
        abilityIsActive = false;
    }
}
