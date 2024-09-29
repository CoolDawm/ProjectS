using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> _baseWeapons;
    [SerializeField]
    private List<Armor> _baseArmors;
    [SerializeField]
    private List<string> _possibleElements = new List<string> { "Fire", "Water", "Earth", "Air", "Lightning" };
    [SerializeField]
    private int _minBonusCount = 1;
    [SerializeField]
    private int _maxBonusCount = 5;
    [SerializeField]
    private float _minBonusValue = 1.0f;
    [SerializeField]
    private float _maxBonusValue = 10.0f;

    public Weapon GenerateRandomWeapon(float charModifier)
    {
        Weapon baseWeapon = _baseWeapons[Random.Range(0, _baseWeapons.Count)];
        Weapon newWeapon = Instantiate(baseWeapon);
        newWeapon._weaponElement = _possibleElements[Random.Range(0, _possibleElements.Count)];
        int bonusCount = Random.Range(_minBonusCount, _maxBonusCount + 1);
        newWeapon.CharacteristicsBonuses().Clear(); 
        for (int i = 0; i < bonusCount; i++)
        {
            float bonusValue = (int) (Random.Range(_minBonusValue, _maxBonusValue)*charModifier);
            newWeapon.CharacteristicsBonuses().Add(bonusValue);
        }
        return newWeapon;
    }

    public Armor GenerateRandomArmor()
    {
        Armor baseArmor = _baseArmors[Random.Range(0, _baseArmors.Count)];
        Armor newArmor = Instantiate(baseArmor);
        return newArmor;
    }
}
