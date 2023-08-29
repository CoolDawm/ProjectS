using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]private float sensitivity=1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private float mouseX; 
    private float adjustedMouseX;
    private float moveHorizontal;
    private float moveVertical;
    
    public float currentSpeed;
    public float stamina;
    public float staminaRecoveryRate;
    public float rollDuration;
    public float rollInvincibilityDuration;
    public float manaGenerationRate;
    public float meleeAbilityRange;
    public float rangedAbilityProjectileLifetime;
    public float areaAbilityRadius;
    public float areaAbilityRange;
    public int healthPotionCount;
    public int manaPotionCount;
    public float healthPotionHealAmount;
    public float manaPotionManaAmount;
    public float healthPotionCooldown;
    public float manaPotionCooldown;
    private Rigidbody rb;
    private float currentStamina;
    private float rollTimer;
    private float rollInvincibilityTimer;
    private float manaTimer;
    private bool isRolling;
    private bool isUsingAbility;
    private bool canUseHealthPotion = true;
    private bool canUseManaPotion = true;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        currentStamina = stamina;
        rollTimer = rollDuration;
        rollInvincibilityTimer = rollInvincibilityDuration;
        manaTimer = manaGenerationRate;
    }

    private void Update()
    {
        // Movement handling with movement speed
        MovePlayer();
        //Player Rotation
        RotatePlayer();
        // Ability usage handling
        UseAbility();

        // Health and mana potion usage handling
        UseHealthPotion();
        UseManaPotion();

        // Stamina recovery and mana generation
        RecoverStamina();
        GenerateMana();
    }
    private void RotatePlayer()
    {
        mouseX = Input.GetAxis("Mouse X");

        // Регулировка чувствительности вращения с помощью мыши
        adjustedMouseX = mouseX * sensitivity;

        // Вращение персонажа в горизонтальной плоскости
        transform.Rotate(Vector3.up, adjustedMouseX);
    }
    private void MovePlayer()
    {
        /*
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        bool isJumping = Input.GetKeyDown(KeyCode.Space);
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement.Normalize();
        currentSpeed = moveSpeed;
        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        // Apply movement to  Rigidbody
        rb.velocity = new Vector3(movement.x * currentSpeed, rb.velocity.y, movement.z * currentSpeed);

        if (isJumping && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            // Aplly force for jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        */
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        bool isJumping = Input.GetKeyDown(KeyCode.Space);
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Получение направления взгляда камеры
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        // Создание вектора направления движения на основе ввода и направления камеры
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement = Quaternion.LookRotation(cameraForward) * movement;
        movement.Normalize();

        currentSpeed = moveSpeed;
        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        // Применение движения к Rigidbody
        rb.velocity = new Vector3(movement.x * currentSpeed, rb.velocity.y, movement.z * currentSpeed);

        if (isJumping && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            // Применение силы для прыжка
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void UseAbility()
    {
        // Check for ability activation key press

        // Check ability type and time of usage

        // Ability application logic based on type
    }

    private void Roll()
    {
        // Roll logic with temporary invincibility
        // Implement your desired roll logic here
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

    private void RecoverStamina()
    {
        // Stamina recovery logic when not rolling or sprinting
        // Implement your desired stamina recovery logic here
    }

    private void GenerateMana()
    {
        // Mana generation logic considering generation and consumption time
        // Implement your desired mana generation logic here
    }
}
