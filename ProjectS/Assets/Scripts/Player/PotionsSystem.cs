using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PotionsSystem : MonoBehaviour
    {
        public float healthPotionHealAmount=5;
        public float manaPotionManaAmount=5;
        public float healthPotionCooldown;
        public float manaPotionCooldown;
        public int healthPotionCount;
        public int manaPotionCount;

        private bool canUseHealthPotion = true;
        private bool canUseManaPotion = true;
        // Start is called before the first frame update
        void Start()
        {
        
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
        if(Input.GetKeyDown(KeyCode.T))
            if (manaPotionManaAmount > 0)
            {
                UseManaPotion(30);
                manaPotionManaAmount--;
            }
            
        }
        private void UseHealthPotion(float health)
        {
            gameObject.GetComponent<HealthSystem>().currentHealth += health;
        }

        private void UseManaPotion(float mana)
        {
            gameObject.GetComponent<PlayerBattleSystem>().currentMana += mana;
        }

    }

