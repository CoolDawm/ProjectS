using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerBattleSystem : MonoBehaviour
    {
        public float manaGenerationRate;
        public float meleeAbilityRange;
        public float rangedAbilityProjectileLifetime;
        public float areaAbilityRadius;
        public float areaAbilityRange;
        private bool isUsingAbility;
        private float manaTimer;
        public float meleeRange = 1f;
        public float projectileSpeed = 10f;
        public float projectileLifetime = 5f;
        public float areaOfEffectRadius = 5f;
        public float summonRange = 10f;
        public GameObject projectilePrefab;       
        // Start is called before the first frame update
        void Start()
        {
            manaTimer = manaGenerationRate;
        }

        
        private void GenerateMana()
        {
            // Mana generation logic considering generation and consumption time
            // Implement your desired mana generation logic here
        }
        

        private void Update()
        {
        GenerateMana();
        // Melee ability
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Geting someone in our melee range
                Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange);

                // Checking for enemies
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                    // Attack mechanic
                        Destroy(collider.gameObject);
                    }
                }
            }

        // Умение: Рендж (Range)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = transform.forward * projectileSpeed;

            Destroy(projectile, projectileLifetime);
        }

        // AOE ability
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Ray from camera to coursor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // If ray lands on enemy, show it
                if (hit.collider.CompareTag("Enemy"))
                {
                    Vector3 targetPosition = hit.collider.transform.position;
                    ShowAbilityArea(targetPosition, areaOfEffectRadius);
                }
                else
                {
                    // If not show where it lands at the end
                    Vector3 targetPosition = hit.point;
                    ShowAbilityArea(targetPosition, areaOfEffectRadius);
                }
            }
        }

        // Ability 4
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // 
            // ...
        }

        
    }

    private void ShowAbilityArea(Vector3 targetPosition, float radius)
    {
        // Show area? Probably will use particles for that
        // ...
    }
}

