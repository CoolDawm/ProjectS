using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SpellBook : ScriptableObject
{
   private AbilitiesHolder _abilityHolder;
   public List<Ability> abilities;
}
