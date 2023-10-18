using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBehaviour : MonoBehaviour
{
    public float _rotationSpeed = 4f;
    public float jumpForce = 1f;
    public bool isRolling;
    public float rollDistance = 5f;
    public float rollCost = 5f;
    [SerializeField]
    private InputActionReference _movementControl;
    [SerializeField]
    private InputActionReference _jumpControl;
    [SerializeField]
    private InputActionReference _sprintControl;
    public float _maxStamina = 10;
    public float _currentStamina = 0f;
    private Transform cameraMain;  
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    private float _currentSpeed;
    private Characteristics _characteristics;
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
        controller = gameObject.GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;
    }
    private void Start()
    {
        _currentStamina = _maxStamina;
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics = gameObject.GetComponent<Characteristics>();
        healthSystem.OnDeath += Die;
    }
    
    private void Update()
    { 
        // Movement handling with movement speed
        MovePlayer();                         
    }
   
    private void MovePlayer()
    {
        if (cameraMain == null)
        {
            cameraMain = Camera.main.transform;
        }
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 movement = _movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x,0,movement.y);
        move = cameraMain.forward * move.z + cameraMain.right * move.x;
        move.y = 0f;
        if (_sprintControl.action.IsPressed()&&_currentStamina>0)
        {
            _currentSpeed = _characteristics.charDic["maxSpeed"];
            //stamina spending
            _currentStamina -= Time.deltaTime*_characteristics.charDic["staminaSpendingRate"];
        }
        else
        {
            _currentSpeed = _characteristics.charDic["movementSpeed"];
            //Stamina recovery
            if (_currentStamina < _maxStamina)
            {
                _currentStamina += Time.deltaTime * _characteristics.charDic["staminaRecoveryRate"];
            }       
        }
        controller.Move(move * Time.deltaTime * _currentSpeed);
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
        //Roll
        if (!isRolling && Input.GetKeyDown(KeyCode.LeftControl)&&_currentStamina>=rollCost)
        {
            Debug.Log("Roll");
            _currentStamina -= rollCost;
            StartCoroutine(Roll());
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    private IEnumerator Roll()
    {
        isRolling = true;
        Vector3 dashDirection = transform.forward;
        Vector3 dashEndPosition = transform.position + dashDirection * rollDistance;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < rollDistance)
        {
            float t = elapsedTime / rollDistance;
            transform.position = Vector3.Lerp(startPosition, dashEndPosition, t);
            elapsedTime += Time.deltaTime*15;
            yield return null;
        }
        isRolling = false;
    }
}
