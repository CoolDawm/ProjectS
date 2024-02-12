using System;
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
    [SerializeField]
    private Image _staminaBarSprite;
    [SerializeField]
    private Image _manaBarSprite;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private Text _staminaText;
    [SerializeField]
    private Text _manaText;
    private bool isUpdating=false;
    public void UpdateHealthBar(float maxHealth,float currentHealth,string tag)
    {
        if (!isUpdating)
        {
            StartCoroutine(UpdateBar(_healthBarSprite, currentHealth, maxHealth,tag));

        }
        else
        {
            //image.fillAmount = currentAm / maxAm;
        }
        //_healthBarSprite.fillAmount = currentHealth/maxHealth;
        _healthText.text = MathF.Round(currentHealth, 0) + "/" + maxHealth;
    }
    public void UpdateShieldBar(float maxShield, float currentShield)
    {
        _shieldBarSprite.fillAmount = currentShield / maxShield;
    }
    public void UpdateStaminaBar(float maxStamina, float currentStamina)
    {
        _staminaBarSprite.fillAmount = currentStamina / maxStamina;
        _staminaText.text = MathF.Round(currentStamina, 0) + "/" + maxStamina;
    }

    public void UpdateManaBar(float maxMana, float currentMana)
    {
        _manaBarSprite.fillAmount = currentMana / maxMana;
        _manaText.text = MathF.Round(currentMana, 0) + "/" + maxMana;
    }

    IEnumerator UpdateBar(Image image, float currentAm, float maxAm,string objTag)
    {
        isUpdating = true;
        if (objTag == "Player")
        {
            image.fillAmount = currentAm / maxAm;
        }
        else
        {
            image.gameObject.SetActive(true);
            image.fillAmount = currentAm / maxAm;
            yield return new WaitForSeconds(1f);
            Debug.Log("Update");
            image.gameObject.SetActive(false);
        }
        isUpdating = false;
    }
}
