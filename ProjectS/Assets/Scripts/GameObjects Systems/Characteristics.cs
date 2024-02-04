using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    public Dictionary<string, float> charDic = new Dictionary<string, float>();
    public Dictionary<string, float> secondCharDic = new Dictionary<string, float>(){};
    public Dictionary<string, float> charBuffBuffer = new Dictionary<string, float>(){};
    public bool isUpdating=false;
    public Action OnUpdate;

    private void Awake()
    {
        secondCharDic.Add("MovementSpeed",0);
        secondCharDic.Add("Defense",0);
        secondCharDic.Add("EvaidChance",0);
        secondCharDic.Add("MaxMana",100);
        secondCharDic.Add("MaxHealth",100);
        secondCharDic.Add("MaxStamina",100);
        secondCharDic.Add("ManaRegen",0);
        secondCharDic.Add("StaminaRegen",0);
        //
        charBuffBuffer.Add("MovementSpeed",0);
        charBuffBuffer.Add("Defense",0);
        charBuffBuffer.Add("EvaidChance",0);
        charBuffBuffer.Add("MaxMana",0);
        charBuffBuffer.Add("MaxHealth",0);
        charBuffBuffer.Add("MaxStamina",0);
        charBuffBuffer.Add("ManaRegen",0);
        charBuffBuffer.Add("StaminaRegen",0);
    }

    private void Start()
    {
        
    }
    //Попробовть Callback
    private void FixedUpdate()
    {
        
        // MovementSpeed and MaxStamina 
        secondCharDic["MovementSpeed"] = 4.5f+charBuffBuffer["MovementSpeed"];
        secondCharDic["MaxStamina"] = charDic["Endurance"] * 7+charBuffBuffer["MaxStamina"];

        //  Defense and EvaidChance 
        secondCharDic["Defense"] = charDic["Endurance"] * 0.4f+charBuffBuffer["Defense"];
        secondCharDic["EvaidChance"] = charDic["Agility"] * 0.4f+charDic["Intellect"] * 0.05f+charBuffBuffer["EvaidChance"];

        //  Mana , Health , Regen
        secondCharDic["MaxMana"] = charDic["Intellect"] * 5+charBuffBuffer["MaxMana"];
        secondCharDic["MaxHealth"] = charDic["Endurance"] * 10+charBuffBuffer["MaxHealth"];
        secondCharDic["ManaRegen"] = charDic["Intellect"] * 0.0075f*charDic["Recovery"]+charBuffBuffer["ManaRegen"];
        secondCharDic["StaminaRegen"] = charDic["Endurance"] * 0.05f*charDic["Recovery"]+charBuffBuffer["StaminaRegen"];
        if (!isUpdating)
        {
            StartCoroutine(CharacteristicUpdateCouroutine(OnUpdate));
        }
    }

    public IEnumerator CharacteristicUpdateCouroutine(Action callback=null)
    {
        isUpdating = true;
        if (callback != null) callback();
        isUpdating = false;
        yield return null;
    }
}
