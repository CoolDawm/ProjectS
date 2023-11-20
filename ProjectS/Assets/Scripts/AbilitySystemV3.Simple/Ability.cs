using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float manaCost;
    public Sprite abilityImage;
    public string description;
    public virtual void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        
    }
}
