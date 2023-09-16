using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class HealthSystem : MonoBehaviour
    {
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    public float currentHealth;
    [SerializeField]
    private float maxShield= 50;
    [SerializeField]
    public float currentShield;
    private HealthBar _healthBar;
    
    void Start()
    {
        currentHealth = maxHealth;
        _healthBar = gameObject.GetComponentInChildren<HealthBar>();       
    }

    public void TakeDamage(float damageAmount)
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
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        _healthBar.UpdateShieldBar(maxShield, currentShield);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void ShieldCharge(float shield)
    {
        currentShield += shield;
        if (currentShield > maxShield)
        {
            currentShield= maxShield;
        }
        _healthBar.UpdateShieldBar(maxShield, currentShield);
    }
    public void HPIncrease(float hp)
    {
        currentHealth += hp;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }
    private void Die()
    {
        
        Destroy(gameObject);
    }
}

