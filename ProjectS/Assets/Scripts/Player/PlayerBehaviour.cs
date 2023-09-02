using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]private float sensitivity=1f;
    [SerializeField] private float _rotationSpeed = 4f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField]
    private InputActionReference _movementControl;
    [SerializeField]
    private InputActionReference _jumpControl;
    [SerializeField]
    private InputActionReference _sprintControl;
    private Transform cameraMain;  
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;     
    public float currentSpeed;
    public float stamina;
    public float staminaRecoveryRate;
    public float rollDuration;
    public float rollInvincibilityDuration;         
    private Rigidbody rb;
    private float currentStamina;
    private float rollTimer;
    private float rollInvincibilityTimer;
    
    private bool isRolling;
    

    private void OnEnable()
    {
        _jumpControl.action.Enable();
        _movementControl.action.Enable();
        _sprintControl.action.Enable();
    }
    private void OnDisable()
    {
        _jumpControl.action.Disable();
        _movementControl.action.Disable();
        _sprintControl.action.Disable();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;
    }
    private void Start()
    {
        currentStamina = stamina;
        rollTimer = rollDuration;
        rollInvincibilityTimer = rollInvincibilityDuration;
        
    }

    private void Update()
    {
        // Movement handling with movement speed
        MovePlayer();                      
        // Stamina recovery and mana generation
        RecoverStamina();
        
    }
   
    private void MovePlayer()
    {
       
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 movement = _movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x,0,movement.y);
        move = cameraMain.forward * move.z + cameraMain.right * move.x;
        move.y = 0f;
        if (_sprintControl.action.IsPressed())
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        controller.Move(move * Time.deltaTime * currentSpeed);
        if (_jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
        }       
        // Changes the height position of the player.. 
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        if(movement!= Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg+cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*_rotationSpeed);
        }
    }

    
    private void Roll()
    {
        // Roll logic with temporary invincibility
        // Implement your desired roll logic here
    }

    
    private void RecoverStamina()
    {
        // Stamina recovery logic when not rolling or sprinting
        // Implement your desired stamina recovery logic here
    }

   
}
