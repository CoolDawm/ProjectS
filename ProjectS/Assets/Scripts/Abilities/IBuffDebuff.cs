using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBuff
{
    float Chance { get; }
    float Power { get; }
    string Effect { get; }
    float Duration { get; }
    void StartEmitting(GameObject emitter, String aim);
}

public interface IDebuff
{
    float Chance { get; }
    float Power { get; }
    string Effect { get; }
    float Duration { get; }
    void StartEmitting(GameObject emitter, String aim);
}