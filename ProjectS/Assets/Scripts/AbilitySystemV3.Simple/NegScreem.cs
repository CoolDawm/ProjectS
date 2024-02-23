using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class NegScreem : AuraV2
{
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range, LayerMask.GetMask(aim));
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<EffectsApplying>().ApplyDebuff(Effect, Power,Duration);
        }
        abilityIsActive = false;
    }
}
