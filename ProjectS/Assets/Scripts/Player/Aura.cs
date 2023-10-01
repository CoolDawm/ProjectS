using System;
using UnityEngine;

    public abstract class Aura : MonoBehaviour, IAura
    {
        public float Chance { get; protected set; }
        public float Range { get; protected set; }
        public float Power { get; protected set; }
        public float Duration { get; protected set; }
        public string Effect { get; protected set; }
        public abstract void StartEmitting(GameObject emitter, String aim);
    }