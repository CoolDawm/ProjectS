using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScream : Aura
{
    private bool isEmitting;

    private void Start()
    {
        Range = 15f;
        Power = 3f;
        Duration = 30f;
        Effect = "movementSpeed";
    }


    public override void StartEmitting(GameObject emitter, String aim)
    {
        Collider[] colliders = Physics.OverlapSphere(emitter.transform.position, Range, LayerMask.GetMask(aim));
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<Characteristics>().ApplyBuff(Effect, Power,Duration);
            }
    }
}
