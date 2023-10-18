using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffDebuff : MonoBehaviour, IBuff
{
    public float Chance { get; protected set; }
    public float Power { get; protected set; }
    public float Duration { get; protected set; }
    public string Effect { get; protected set; }
    public abstract void StartEmitting(GameObject emitter, String aim);
}