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
    public float rollSpeed = 10f;
    public float rollDistance = 0.5f;
    public float rollCost = 5f;
    [SerializeField]
    private InputActionReference _movementControl;
    [SerializeField]
    private InputActionReference _jumpControl;
    [SerializeField]
    private InputActionReference _sprintControl;
    [SerializeField]
    private InputActionReference _rollControl;
    private float _currentStamina;
    private Transform _cameraMain;  
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private float _gravityValue = -25f;
    private float _currentSpeed;
    private Characteristics _characteristics;
    private GameObject _afterDeathPanel;
    private Animator _animator;
    private Vector2 _movement; 
    private void OnEnable()
    {
        _jumpControl.action.Enable();
        _movementControl.action.Enable();
        _sprintControl.action.Enable();
        _rollControl.action.Enable();
    }
    private void OnDisable()
    {
        _jumpControl.action.Disable();
        _movementControl.action.Disable();
        _sprintControl.action.Disable();
        _rollControl.action.Disable();
    }
    private void Awake()
    {
        _controller = gameObject.GetComponentInChildren<CharacterController>();
        _cameraMain = Camera.main.transform;
    }
    private void Start()
    {
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics = gameObject.GetComponent<Characteristics>();
        _currentStamina = 100;
        _afterDeathPanel = Resources.Load<GameObject>("Prefabs/UI/AfterDeathUICanvas");
        healthSystem.OnDeath += Die;
        healthSystem.OnTakeDamage += TakeDamageAnim;
        _animator = GetComponentInChildren<Animator>();
        Debug.Log(_animator);
    }
    
    private void Update()
    { 
        MovePlayer();
    }
   
    private void MovePlayer()
    {
        if (_cameraMain == null)
        {
            _cameraMain = Camera.main.transform;
        }
        _groundedPlayer = IsGrounded();
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        if (_controller.isGrounded)
        {
            _animator.SetBool("IsGrounded",true);
        }
        //Roll
        if ( _rollControl.action.triggered &&!isRolling&&_currentStamina>=rollCost)
        {
            Debug.Log("Roll");
            _currentStamina -= rollCost;
            StartCoroutine(Roll());
        }
        //Jump
        if (_jumpControl.action.triggered && _groundedPlayer)
        {
            _animator.SetBool("IsGrounded",false);
            _playerVelocity.y += Mathf.Sqrt(jumpForce * -1.0f * _gravityValue);
        }       
        // Changes the height position of the player.. 
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
        //Movement
        if (_movementControl.action.triggered)
        {
            _movement = _movementControl.action.ReadValue<Vector2>();
        }
        if (_movement != Vector2.zero)
        {
            
            if (_sprintControl.action.IsPressed()&&_currentStamina>0)
            {
                _currentSpeed = _characteristics.charDic["maxSpeed"];
                _animator.SetFloat("Speed", 1);
                //stamina spending
                _currentStamina -= Time.deltaTime*_characteristics.charDic["staminaSpendingRate"];
            }
            else
            {
                _currentSpeed = _characteristics.charDic["movementSpeed"];
                _animator.SetFloat("Speed", 0.5f);
                //Stamina recovery
                if (_currentStamina < _characteristics.charDic["stamina"])
                {
                    _currentStamina += Time.deltaTime * _characteristics.charDic["staminaRecoveryRate"];
                }       
            }
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }
        Vector3 move = new Vector3(_movement.x,0,_movement.y);
        move = _cameraMain.forward * move.z + _cameraMain.right * move.x;
        move.y = 0f;
        if (!isRolling)
        {
            _controller.Move(move * Time.deltaTime * _currentSpeed);
        }
        if(_movement!= Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(_movement.x, _movement.y) * Mathf.Rad2Deg+_cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*rotationSpeed);
        }
    }
   
    private IEnumerator Roll()
    {
        isRolling = true;
        if (_animator.GetFloat("Speed") == 0)
        {
            _animator.SetFloat("DashSpeed", 0.5f);
        }
        else
        {
            _animator.SetFloat("DashSpeed", _animator.GetFloat("Speed"));
        }
        Vector3 dashDirection = transform.forward;
        float elapsedTime = 0f;
        while (elapsedTime < rollDistance)
        {
            float t = elapsedTime / rollDistance;
            _controller.Move(dashDirection * Time.deltaTime * rollSpeed); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
        _animator.SetFloat("DashSpeed", 0);
    }
    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Instantiate(_afterDeathPanel, null);
        Destroy(gameObject);
    }
    private bool IsGrounded()
    {
        float rayDistance = 0.1f; // расстояние, на которое будет выпущен луч
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider != null && hit.collider.tag == "Ground") 
            {
                Debug.DrawRay(transform.position, Vector3.down,Color.magenta);
                return true;
            }
        }
        
        return false;
    }
    //Animations Triggers
    public void TakeDamageAnim()
    {
        _animator.SetTrigger("IsGetHurt");
    }
    public void AttackAnim(String attackTrigger)
    {
        _animator.SetTrigger(attackTrigger);
    }
}
