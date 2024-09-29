using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public bool abilityIsActive = false; 
    public new string name;
    public float manaCost;
    public float cooldown;
    public Sprite abilityImage;
    public string description;
    public string animName;
    public float range;
    public float animTime;
    public string aim;
    public float strongAttackStaminaCost;
    public string strongAttackName;
    public float strongAttackTime;
    public virtual void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        
    }
    public virtual void Activate(GameObject user,CoroutineRunner coroutineRunner,Animator animator)
    {
        
    }

    public virtual void Activate(GameObject user, CoroutineRunner coroutineRunner, GameObject target)
    {
        
    }
    public virtual IEnumerator ActivateStrongAttack(GameObject user, CoroutineRunner coroutineRunner)
    {
        yield break;
    }

}
