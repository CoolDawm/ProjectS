using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVPSystems
{
   
    public class PlayerBattleSystem : MonoBehaviour
    {
        public float manaGenerationRate;
        public float meleeAbilityRange;
        public float rangedAbilityProjectileLifetime;
        public float areaAbilityRadius;
        public float areaAbilityRange;
        private bool isUsingAbility;
        private float manaTimer;
        // Start is called before the first frame update
        void Start()
        {
            manaTimer = manaGenerationRate;
        }

        // Update is called once per frame
        void Update()
        {
            // Ability usage handling
            UseAbility();
            GenerateMana();
        }
        private void UseAbility()
        {
            // Check for ability activation key press

            // Check ability type and time of usage

            // Ability application logic based on type
        }
        private void GenerateMana()
        {
            // Mana generation logic considering generation and consumption time
            // Implement your desired mana generation logic here
        }
    }
}
