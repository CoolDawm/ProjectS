using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    [SerializeField]
    public int damage;
    [SerializeField]
    public float range;// now its only melee range
    [SerializeField]
    public GameObject weaponPrefab;
    [SerializeField]
    private Ability _weaponAbility;
    [SerializeField]
    private List<float> _characteristicsBonuses;
    public Vector3 offset;
    public string _weaponElement;
    public Ability GetAbility()
    {
        _weaponAbility.range = range;
        return _weaponAbility;
    }
    public List<float> CharacteristicsBonuses()
    {
        return _characteristicsBonuses;
    }
}