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
    public Skill skill;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
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
    private float _terminalVelocity = 53.0f;
    private float _verticalVelocity;
    private float _currentSpeed;
    private float _previousBlend;
    private float _previousSpeed = 0;
    private float smoothness = 2f;
    private float _animBlend;
    private CoroutineRunner _coroutineRunner;
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
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
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
        JumpAndGravity();
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

        if (!skill.isWorking)
        {
            _animator.SetFloat("DashSpeed", 0);
        }
        if (_controller.isGrounded)
        {
            _animator.SetBool("IsGrounded",true);
        }
        //Roll
        if ( _rollControl.action.triggered &&!skill.isWorking&&_currentStamina>= skill.staminaCost)
        {
            if (_animator.GetFloat("Speed") == 0)
            {
                _animator.SetFloat("DashSpeed", 0.5f);
            }
            else
            {
                _animator.SetFloat("DashSpeed", _animator.GetFloat("Speed"));
            }
            skill.Activate(gameObject,_coroutineRunner);
            Debug.Log("Roll");
            _currentStamina -= skill.staminaCost;
        }
        if (_movementControl.action.triggered)
        {
            _movement = _movementControl.action.ReadValue<Vector2>();
        }
        if (_movement != Vector2.zero)
        {
            if (_sprintControl.action.IsPressed()&&_currentStamina>0)
            {
                //stamina spending
                _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.charDic["maxSpeed"], Time.deltaTime * smoothness);
                _animBlend =1f;
                _currentStamina -= Time.deltaTime*_characteristics.charDic["staminaSpendingRate"];
            }
            else
            {
                _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.charDic["movementSpeed"], Time.deltaTime * smoothness);
                _animBlend =0.5f;
                //Stamina recovery
                if (_currentStamina < _characteristics.charDic["stamina"])
                {
                    _currentStamina += Time.deltaTime * _characteristics.charDic["staminaRecoveryRate"];
                }       
            }
            _animator.SetFloat("Speed", Mathf.Lerp(_previousBlend, _animBlend, Time.deltaTime * (smoothness+2)));
            _previousBlend = _animator.GetFloat("Speed");
            _previousSpeed = _currentSpeed;
        }
        else
        {
            _animator.SetFloat("Speed", Mathf.Lerp(_previousBlend, 0, Time.deltaTime ));
            _previousBlend = _animator.GetFloat("Speed");
            _currentSpeed = 0;
        }
        Vector3 move = new Vector3(_movement.x,0,_movement.y);
        move = _cameraMain.forward * move.z + _cameraMain.right * move.x;
        move.y = 0f;
        if (!skill.isWorking)
        {
            _controller.Move(move *  (_currentSpeed  * Time.deltaTime)+
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
        if(_movement!= Vector2.zero&&!skill.isWorking)
        {
            float targetAngle = Mathf.Atan2(_movement.x, _movement.y) * Mathf.Rad2Deg+_cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*rotationSpeed);
        }
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
     private void JumpAndGravity()
        {
            if (_groundedPlayer)
            {
               
                _animator.SetBool("IsGrounded",true);
                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_jumpControl.action.triggered)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                        _animator.SetBool("IsGrounded",false);
                }

               
            }
           
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
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
