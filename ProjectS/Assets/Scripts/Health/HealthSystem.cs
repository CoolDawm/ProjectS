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
    private HealthBar _healthBar;
    private string _objectTag;
    public Action OnDeath;
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
        _floatingTextManager.ShowFloatingText(gameObject,"+"+hp+"HP");
    }
    public void TakeDamage(float damageAmount)
    {
        if (_objectTag == "Player")
        {
            if (gameObject.GetComponent<PlayerBehaviour>().isRolling)
            {
                _floatingTextManager.ShowFloatingText(gameObject,"Dodge");
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
                _floatingTextManager.ShowFloatingNumbers(gameObject,damageAmount);
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
            _floatingTextManager.ShowFloatingNumbers(gameObject,damageAmount);
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
        _floatingTextManager.ShowFloatingText(gameObject,"Shield Charged");
        _healthBar.UpdateShieldBar(maxShield, currentShield);
    }
    public void HpIncrease(float hp)
    {
        currentHealth += hp;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        _floatingTextManager.ShowFloatingText(gameObject,"HP UP");
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }
    
    private void Die()
    {
        OnDeath?.Invoke();
    }
}

