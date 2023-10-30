using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBehaviour : MonoBehaviour
{
    public float rotationSpeed = 4f;
    public float jumpForce = 0.5f;
    public bool isRolling;
    public float rollDistance = 5f;
    public float rollCost = 5f;
    [SerializeField]
    private InputActionReference _movementControl;
    [SerializeField]
    private InputActionReference _jumpControl;
    [SerializeField]
    private InputActionReference _sprintControl;
    private float _currentStamina;
    private Transform _cameraMain;  
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private float _gravityValue = -25f;
    private float _currentSpeed;
    private Characteristics _characteristics;
    private GameObject _afterDeathPanel;
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
        _controller = gameObject.GetComponent<CharacterController>();
        _cameraMain = Camera.main.transform;
    }
    private void Start()
    {
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics = gameObject.GetComponent<Characteristics>();
        _currentStamina = 100;
        _afterDeathPanel = Resources.Load<GameObject>("Prefabs/UI/AfterDeathUICanvas");
        healthSystem.OnDeath += Die;
    }
    
    private void Update()
    { 
        // Movement handling with movement speed
        MovePlayer();                         
    }
   
    private void MovePlayer()
    {
        if (_cameraMain == null)
        {
            _cameraMain = Camera.main.transform;
        }
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
        Vector2 movement = _movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x,0,movement.y);
        move = _cameraMain.forward * move.z + _cameraMain.right * move.x;
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
            if (_currentStamina < _characteristics.charDic["stamina"])
            {
                _currentStamina += Time.deltaTime * _characteristics.charDic["staminaRecoveryRate"];
            }       
        }
        _controller.Move(move * Time.deltaTime * _currentSpeed);
        if (_jumpControl.action.triggered && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(jumpForce * -1.0f * _gravityValue);
        }       
        // Changes the height position of the player.. 
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
        if(movement!= Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg+_cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*rotationSpeed);
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Instantiate(_afterDeathPanel, null);
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
