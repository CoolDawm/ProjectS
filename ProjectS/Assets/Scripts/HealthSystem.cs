using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class HealthSystem : MonoBehaviour
    {
    public float maxHealth = 100;
    [SerializeField]
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        Destroy(gameObject);
    }
}

