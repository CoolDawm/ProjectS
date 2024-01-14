using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [HideInInspector] public bool abilityIsActive = false; 
    public new string name;
    public float manaCost;
    public float cooldown;
    public Sprite abilityImage;
    public string description;
    public string animName;
    public float range;
    public float animTime;
    public virtual void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        
    }
}
