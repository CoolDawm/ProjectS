using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private bool _isManequin = false;
    [SerializeField]
    private bool _isSpawner = false;
    [SerializeField]
    private bool _hasShield = true;

    public float maxHealth = 100;
    public float currentHealth;
    public float maxShield = 50;
    public float currentShield;
    public FloatTextManager floatingTextManager;
    public Action OnDeath;
    public Action OnTakeDamage;
    public HealthBar healthBar { get; private set; }

    private string _objectTag;
    private Characteristics _characteristics;
    private Utilities _utilities = new Utilities();
    private Coroutine _shieldRegenCoroutine;

    void Start()
    {
        _characteristics = GetComponent<Characteristics>();
        if (!_isManequin &&!_isSpawner)
        {
            maxHealth = _characteristics.secondCharDic["MaxHealth"];
        }
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
        InvokeRepeating("UpdateHealth", 2, 1);
    }

    public void IncreaseCurrentHealth(float hp)
    {
        if (currentHealth + hp > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += hp;
        }
        healthBar.UpdateHealthBar(maxHealth, currentHealth, gameObject.tag);
        WorNotification(Color.red, "+" + hp + "HP");
    }

    public void UpdateHealth()
    {
        if (_isManequin || _isSpawner) return;
        maxHealth = _characteristics.secondCharDic["MaxHealth"];
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damageAmount, Color color)
    {
        if (_isSpawner)
        {
            ApplyDamage(damageAmount, color);

        }
        else
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
                    ApplyDamage(damageAmount, color);
                }
            }
            else
            {
                ApplyDamage(damageAmount, color);
            }
            if (_shieldRegenCoroutine != null)
            {
                StopCoroutine(_shieldRegenCoroutine);
            }
            _shieldRegenCoroutine = StartCoroutine(ShieldRegenCoroutine());
        }
        
    }

    private void ApplyDamage(float damageAmount, Color color)
    {
        if (_hasShield && currentShield > 0)
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

        NumNotification(color, damageAmount);
        healthBar.UpdateShieldBar(maxShield, currentShield);
        healthBar.UpdateHealthBar(maxHealth, currentHealth, gameObject.tag);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void NumNotification(Color color, float number)
    {
        floatingTextManager.ShowFloatingNumbers(gameObject.transform, number, color);
    }

    public void WorNotification(Color color, string word)
    {
        floatingTextManager.ShowFloatingText(gameObject.transform, word, color);
    }

    public void ShieldCharge(float shield)
    {
        currentShield += shield;
        if (currentShield > maxShield)
        {
            currentShield = maxShield;
        }
        floatingTextManager.ShowFloatingText(gameObject.transform, "Shield Charged", Color.blue);
        healthBar.UpdateShieldBar(maxShield, currentShield);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        floatingTextManager.ShowFloatingText(gameObject.transform, "Died", Color.black);
    }

    private IEnumerator ShieldRegenCoroutine()
    {
        yield return new WaitForSeconds(3f);
        while (_hasShield && currentShield < maxShield)
        {
            currentShield += 5f; 
            healthBar.UpdateShieldBar(maxShield, currentShield);
            yield return new WaitForSeconds(1f);
        }
    }
}
