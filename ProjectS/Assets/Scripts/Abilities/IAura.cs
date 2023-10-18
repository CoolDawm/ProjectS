using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAura
{
    float Chance { get; }
    float Range { get; }
    float Power { get; }
    string Effect { get; }
    float Duration { get; }
    void StartEmitting(GameObject emitter, String aim);
    
}



