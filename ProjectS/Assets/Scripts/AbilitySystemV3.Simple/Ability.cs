using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float manaCost;
    public Sprite AbilityImage;
    public virtual void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        
    }
}
