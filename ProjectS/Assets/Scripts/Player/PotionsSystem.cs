using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PotionsSystem : MonoBehaviour
    {
        public float healthPotionHealAmount;
        public float manaPotionManaAmount;
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
            UseHealthPotion();
            UseManaPotion();
        }
        private void UseHealthPotion()
        {
            // Check for health potion usage key press

            // Check potion availability and cooldown

            // Health restoration logic and cooldown set
        }

        private void UseManaPotion()
        {
            // Check for mana potion usage key press

            // Check potion availability and cooldown

            // Mana restoration logic and cooldown set
        }

    }

