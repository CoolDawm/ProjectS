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
    public FloatTextManager _floatingTextManager;
    public Action OnDeath;
    private HealthBar _healthBar;
    private string _objectTag;
    void Start()
    {
        currentHealth = maxHealth;
        _healthBar = gameObject.GetComponentInChildren<HealthBar>();
        _objectTag = gameObject.tag;
        _floatingTextManager = GameObject.FindGameObjectWithTag("FloatingTextManager").GetComponent<FloatTextManager>();
    }
    public void IncreaseCurrentHealth(float hp) {
        currentHealth += hp;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        _floatingTextManager.ShowFloatingText(gameObject.transform,"+"+hp+"HP");
    }
    public void TakeDamage(float damageAmount)
    {
        if (_objectTag == "Player")
        {
            if (gameObject.GetComponent<PlayerBehaviour>().isRolling)
            {
                _floatingTextManager.ShowFloatingText(gameObject.transform,"Dodge");
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
                        currentShield = 0;
                    }
                }
                else
                {
                    currentHealth -= damageAmount;
                }
                _floatingTextManager.ShowFloatingNumbers(gameObject.transform,damageAmount);
                _healthBar.UpdateHealthBar(maxHealth, currentHealth);
                _healthBar.UpdateShieldBar(maxShield, currentShield);
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
                    currentHealth -= (damageAmount - currentShield);
                    currentShield = 0;
                }
            }
            else
            {
                currentHealth -= damageAmount;
            }
            _floatingTextManager.ShowFloatingNumbers(gameObject.transform,damageAmount);
            _healthBar.UpdateHealthBar(maxHealth, currentHealth);
            _healthBar.UpdateShieldBar(maxShield, currentShield);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
    }
    public void ShieldCharge(float shield)
    {
        currentShield += shield;
        if (currentShield > maxShield)
        {
            currentShield= maxShield;
        }
        _floatingTextManager.ShowFloatingText(gameObject.transform,"Shield Charged");
        _healthBar.UpdateShieldBar(maxShield, currentShield);
    }
    private void Die()
    {
        OnDeath?.Invoke();
    }
}

