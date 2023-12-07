using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public new string name;
    public float staminaCost;
    public float cooldown;
    public Sprite skillImage;
    public string description;
    public bool isWorking;
    public float workingTime;
    public virtual void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        
    }
}
