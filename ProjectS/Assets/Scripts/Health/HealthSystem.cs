using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;


public class HealthSystem : MonoBehaviour
    {
    public float maxHealth = 100;    
    public float currentHealth;
    public float maxShield= 50;
    public float currentShield;
    public FloatTextManager floatingTextManager;
    public Action OnDeath;
    public Action OnTakeDamage;
    public HealthBar healthBar { get; private set; }
    private string _objectTag;
    private Characteristics _characteristics;
    private Utilities _utilities=new Utilities();
    void Start()
    {
        _characteristics = GetComponent<Characteristics>();
        maxHealth = _characteristics.secondCharDic["MaxHealth"];
        currentHealth = maxHealth;
        
        _objectTag = gameObject.tag;
        if (_objectTag == "Player")
        {
            healthBar = GameObject.FindWithTag("PlayerHUD").GetComponent<HealthBar>();
        }
        else
        {
            healthBar = gameObject.GetComponentInChildren<HealthBar>();
        }
        floatingTextManager = GameObject.FindGameObjectWithTag("FloatingTextManager").GetComponent<FloatTextManager>();
        InvokeRepeating("UpdateHealth",2,1);
    }
    public void IncreaseCurrentHealth(float hp)
    {

        if (currentHealth+hp> maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.UpdateHealthBar(maxHealth, currentHealth,gameObject.tag);
        }
        else
        {
            currentHealth += hp;
            healthBar.UpdateHealthBar(maxHealth, currentHealth,gameObject.tag);

        }
        WorNotification(Color.red, "+" + hp + "HP");

    }

    public void UpdateHealth()
    {
        maxHealth = _characteristics.secondCharDic["MaxHealth"];
        if (currentHealth> maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damageAmount,Color color)
    {
        if (_utilities.CalculateChance(_characteristics.secondCharDic["EvaidChance"]))
        {
            WorNotification(Color.blue, "Dodge");
            return;
        }
        float defense = _characteristics.secondCharDic["Defense"];
        damageAmount -= defense * 0.3f;
        if (_objectTag == "Player")
        {
            if (gameObject.GetComponent<PlayerBehaviour>().skill.isWorking)
            {
                WorNotification(Color.blue, "Dodge");
            }
            else
            {
                if (currentShield != 0)
                {
                    if (currentShield >= damageAmount)
                    {
                        currentShield -= damageAmount;
                    }
                    else
                    {

                        currentHealth -= (damageAmount - currentShield);
                        OnTakeDamage?.Invoke();
                        currentShield = 0;
                    }
                }
                else
                {
                    currentHealth -= damageAmount;
                        OnTakeDamage?.Invoke();
                }
                NumNotification(color,damageAmount);
                healthBar.UpdateShieldBar(maxShield, currentShield);
                healthBar.UpdateHealthBar(maxHealth, currentHealth,gameObject.tag);
                if (currentHealth <= 0)
                {
                    Die();
                }
            }
            
        }
        else
        {
            if (currentShield != 0)
            {
                if (currentShield >= damageAmount)
                {
                    currentShield -= damageAmount;
                }
                else
                {
                    OnTakeDamage?.Invoke();
                    currentHealth -= (damageAmount - currentShield);
                    currentShield = 0;
                }
            }
            else
            {
                OnTakeDamage?.Invoke();
                currentHealth -= damageAmount;
            }
            NumNotification(color,damageAmount);
            healthBar.UpdateShieldBar(maxShield, currentShield);
            healthBar.UpdateHealthBar(maxHealth, currentHealth,gameObject.tag);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
    }

    public void NumNotification(Color color,float number)
    {
        floatingTextManager.ShowFloatingNumbers(gameObject.transform,number,color);
    }

    public void WorNotification(Color color,string word)
    {
        floatingTextManager.ShowFloatingText(gameObject.transform,word,color);
    }
    public void ShieldCharge(float shield)
    {
        currentShield += shield;
        if (currentShield > maxShield)
        {
            currentShield= maxShield;
        }
        floatingTextManager.ShowFloatingText(gameObject.transform,"Shield Charged",Color.blue);
        healthBar.UpdateShieldBar(maxShield, currentShield);
    }
    private void Die()
    {
        OnDeath?.Invoke();
        floatingTextManager.ShowFloatingText(gameObject.transform,"Died",Color.black);
    }
}

