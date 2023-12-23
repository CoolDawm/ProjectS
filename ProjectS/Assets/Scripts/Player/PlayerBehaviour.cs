using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBehaviour : MonoBehaviour
{
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.8f;
    [SerializeField] private InputActionReference _movementControl;
    [SerializeField] private InputActionReference _jumpControl;
    [SerializeField] private InputActionReference _sprintControl;
    [SerializeField] private InputActionReference _rollControl;
    public float rotationSpeed = 4f;
    public Skill skill;
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
    private float _smoothness = 2f;
    private float _animBlend;
    private CoroutineRunner _coroutineRunner;
    private Characteristics _characteristics;
    private GameObject _afterDeathPanel;
    private Animator _animator;
    private Vector2 _movement;
    private float _fallDistance=0;
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
        JumpAndGravity();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (_cameraMain == null)
        {
            _cameraMain = Camera.main.transform;
        }
        if (!skill.isWorking)
        {
            _animator.SetFloat("DashSpeed", 0);
        }
        //Roll
        if (_rollControl.action.triggered && !skill.isWorking && _currentStamina >= skill.staminaCost)
        {
            skill.Activate(gameObject, _coroutineRunner);
            Debug.Log("Roll");
            _currentStamina -= skill.staminaCost;
        }

        if (skill.isWorking)
        {
            _animator.SetFloat("DashSpeed", 1);
        }
        else
        {
            _animator.SetFloat("DashSpeed", 0);
        }

        if (_movementControl.action.triggered)
        {
            _movement = _movementControl.action.ReadValue<Vector2>();
        }

        if (_movement != Vector2.zero)
        {
            if (_sprintControl.action.IsPressed() && _currentStamina > 0)
            {
                //stamina spending
                _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.charDic["maxSpeed"],
                    Time.deltaTime );
                _animBlend = 1f;
                _currentStamina -= Time.deltaTime * _characteristics.charDic["staminaSpendingRate"];
            }
            else
            {
                _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.charDic["movementSpeed"],
                    Time.deltaTime);
                _animBlend = 0.5f;
                //Stamina recovery
                if (_currentStamina < _characteristics.charDic["stamina"])
                {
                    _currentStamina += Time.deltaTime * _characteristics.charDic["staminaRecoveryRate"];
                }
            }

            _animator.SetFloat("Speed", Mathf.Lerp(_previousBlend, _animBlend, Time.deltaTime * (_smoothness + 2)));
            _previousBlend = _animator.GetFloat("Speed");
            _previousSpeed = _currentSpeed;
        }
        else
        {
            _animator.SetFloat("Speed", Mathf.Lerp(_previousBlend, 0, Time.deltaTime));
            _previousBlend = _animator.GetFloat("Speed");
            _currentSpeed = 0;
        }

        Vector3 move = new Vector3(_movement.x, 0, _movement.y);
        move = _cameraMain.forward * move.z + _cameraMain.right * move.x;
        move.y = 0f;
        if (!skill.isWorking)
        {
            _controller.Move(move * (_currentSpeed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        if (_movement != Vector2.zero && !skill.isWorking)
        {
            float targetAngle = Mathf.Atan2(_movement.x, _movement.y) * Mathf.Rad2Deg + _cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
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
        float rayDistance = 0.1f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                Debug.DrawRay(transform.position, Vector3.down, Color.magenta);
                _animator.SetBool("IsGrounded", true);
                _fallDistance = 0f;
                return true;
            }
        }
        _animator.SetBool("IsGrounded", false);
        return false;
    }

    private void JumpAndGravity()
    {
        if (IsGrounded())
        {
            
            _animator.SetBool("IsFalling",false);
            // stop  velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }
            // Jump
            if (_jumpControl.action.triggered)
            {
                //_animator.SetBool("IsJumping",true);
                _animator.SetTrigger("Jump");
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }
        else
        {
            if (_fallDistance > 1.5f)
            {
                _animator.SetBool("IsFalling", true);
                //_animator.SetBool("IsJumping", false);
            }
            else
            {
                _animator.SetBool("IsFalling", false);
                //_animator.SetBool("IsJumping", false);
            }
        }
        // calculate fall distance
        if (_verticalVelocity < 0)
        {
            _fallDistance += Mathf.Abs(_verticalVelocity) * Time.deltaTime;
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
