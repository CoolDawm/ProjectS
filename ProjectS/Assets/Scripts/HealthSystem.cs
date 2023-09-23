using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


    public class HealthSystem : MonoBehaviour
    {
    public float maxHealth = 100;    
    public float currentHealth;
    public float maxShield= 50;
    public float currentShield;
    private HealthBar _healthBar;
    private string _objectTag;
    void Start()
    {
        currentHealth = maxHealth;
        _healthBar = gameObject.GetComponentInChildren<HealthBar>();
        _objectTag = gameObject.tag;
    }
    public void IncreaseCurrentHealth(float hp) {
        currentHealth += hp;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }
    public void TakeDamage(float damageAmount)
    {
        if (_objectTag == "Player")
        {
            if (gameObject.GetComponent<PlayerBehaviour>().isRolling)
            {
                Debug.Log("Missed");
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

