
using UnityEngine;


public class PotionsSystem : MonoBehaviour
{
    public float healthPotionHealAmount = 5;
    public float manaPotionManaAmount = 5;
    private HealthSystem _healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        _healthSystem = gameObject.GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Health and mana potion usage handling
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (healthPotionHealAmount > 0)
            {
                UseHealthPotion(30);
                healthPotionHealAmount--;
            }

        }
        if (Input.GetKeyDown(KeyCode.T))
            if (manaPotionManaAmount > 0)
            {
                UseManaPotion(30);
                manaPotionManaAmount--;
            }
    }
    private void UseHealthPotion(float health)
    {
        _healthSystem.IncreaseCurrentHealth(health);
    }
    private void UseManaPotion(float mana)
    {
       
    }

}

