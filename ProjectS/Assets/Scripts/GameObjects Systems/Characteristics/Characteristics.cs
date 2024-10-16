using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    public Dictionary<string, float> charDic = new Dictionary<string, float>();
    public Dictionary<string, float> secondCharDic = new Dictionary<string, float>() { };
    public Dictionary<string, float> charBuffBuffer = new Dictionary<string, float>() { };
    public Dictionary<string, float> resistsDic = new Dictionary<string, float>();

    public bool isUpdating = false;
    public Action OnUpdate;
    private float _weaponDamage = 1;// maybe not player only
    private int _freePoints = 5;

    private void Awake()
    {
        secondCharDic.Add("MovementSpeed", 0);
        secondCharDic.Add("Defense", 0);
        secondCharDic.Add("EvaidChance", 0);
        secondCharDic.Add("MaxMana", 1000);
        secondCharDic.Add("MaxHealth", 1000);
        secondCharDic.Add("MaxStamina", 1000);
        secondCharDic.Add("ManaRegen", 0);
        secondCharDic.Add("StaminaRegen", 0);
        secondCharDic.Add("MeleeDamageMultiplyer", 0);
        //
        charBuffBuffer.Add("MovementSpeed", 0);
        charBuffBuffer.Add("Defense", 0);
        charBuffBuffer.Add("EvaidChance", 0);
        charBuffBuffer.Add("MaxMana", 0);
        charBuffBuffer.Add("MaxHealth", 0);
        charBuffBuffer.Add("MaxStamina", 0);
        charBuffBuffer.Add("ManaRegen", 0);
        charBuffBuffer.Add("StaminaRegen", 0);
        Debug.Log($"Char here {gameObject.name}");
        resistsDic.Add("Fire",10);
        resistsDic.Add("Ice",10);
        resistsDic.Add("Earth",10);
        resistsDic.Add("Air",10);
        resistsDic.Add("Thunder", 10);

    }


    private void FixedUpdate()
    {

        // MovementSpeed and MaxStamina 
        secondCharDic["MovementSpeed"] = 4.5f + charBuffBuffer["MovementSpeed"];
        secondCharDic["MaxStamina"] = charDic["Endurance"] * 7 + charBuffBuffer["MaxStamina"];


        //  Defense and EvaidChance 
        secondCharDic["Defense"] = charDic["Endurance"] * 0.4f + charBuffBuffer["Defense"];
        secondCharDic["EvaidChance"] = charDic["Agility"] * 0.4f + charDic["Intellect"] * 0.05f + charBuffBuffer["EvaidChance"];

        //  Mana , Health , Regen
        secondCharDic["MaxMana"] = charDic["Intellect"] * 5 + charBuffBuffer["MaxMana"];
        secondCharDic["MaxHealth"] = charDic["Endurance"] * 10 + charBuffBuffer["MaxHealth"];
        secondCharDic["ManaRegen"] = charDic["Intellect"] * 0.0075f * charDic["Recovery"] + charBuffBuffer["ManaRegen"];
        secondCharDic["StaminaRegen"] = charDic["Endurance"] * 0.1f * charDic["Recovery"] + charBuffBuffer["StaminaRegen"];
        secondCharDic["MeleeDamageMultiplyer"] = 1 + (charDic["Strength"] + charDic["Agility"]) * 0.03f;
        if (!isUpdating)
        {
            StartCoroutine(CharacteristicUpdateCouroutine(OnUpdate));
        }
    }
    /*private Dictionary<string,float> GetUpdatedFirstDivisionCharacteristics()
    {
        return charDic;
    }*/
    public IEnumerator CharacteristicUpdateCouroutine(Action callback = null)
    {
        isUpdating = true;
        if (callback != null) callback();
        isUpdating = false;
        yield return null;
    }
    public void MultiplyAttributes(float multiplier)
    {
        foreach (var key in charDic.Keys.ToList())
        {
            charDic[key] *= multiplier;

        }
        secondCharDic["Defense"] = charDic["Endurance"] * 0.4f + charBuffBuffer["Defense"];

    }
    public void AddAttributes(List<float> attributes)
    {
        int i = 0;
        foreach (var key in charDic.Keys.ToList())
        {
            Debug.Log(key + "+=" + attributes[i]);
            charDic[key] += attributes[i];//could be bade if 6 characteristics(in diff switcher only 5 attributes)
            i++;
        }
        secondCharDic["Defense"] = charDic["Endurance"] * 0.4f + charBuffBuffer["Defense"];
        OnUpdate?.Invoke();
    }
    public void RemoveAttributes(List<float> attributes)
    {
        int i = 0;
        foreach (var key in charDic.Keys.ToList())
        {
            Debug.Log(key + "-=" + attributes[i]);
            charDic[key] -= attributes[i];//could be bade if 6 characteristics(in diff switcher only 5 attributes)
            i++;
        }
        secondCharDic["Defense"] = charDic["Endurance"] * 0.4f + charBuffBuffer["Defense"];
        OnUpdate?.Invoke();

    }
    public void SetWeaponDamage(float damage)
    {
        _weaponDamage = damage;
    }
    public float GetWeaponDamage()
    {
        return _weaponDamage;
    }
    public void AddFreePoints(int amount)
    {
        _freePoints += amount;
    }
    public int GetFreePointsAmount()
    {
        return _freePoints;
    }
    public void AddPointToCharacteristic(string charName)
    {
        charDic[charName]++;
        _freePoints--;
        OnUpdate?.Invoke();
    }
    public void RemovePointFromCharacteristic(string charName)
    {
        charDic[charName]--;
        _freePoints++;
        OnUpdate?.Invoke();
    }
}
