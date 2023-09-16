using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthBarSprite;
    [SerializeField]
    private Image _shieldBarSprite;
    public void UpdateHealthBar(float maxHealth,float currentHealth)
    {
        _healthBarSprite.fillAmount = currentHealth/maxHealth;
    }
    public void UpdateShieldBar(float maxShield, float currentShield)
    {
        _shieldBarSprite.fillAmount = currentShield / maxShield;
    }
}
