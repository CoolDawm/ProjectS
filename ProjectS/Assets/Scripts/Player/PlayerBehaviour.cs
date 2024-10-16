using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

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
    public float rotationSpeed = 6f;
    public Skill skill;
    private float _currentStamina;
    private Transform _cameraMain;
    private CharacterController _controller;
    private bool _groundedPlayer;//Need to call IsGrounder in Update only once and use this instead
    private float _terminalVelocity = 53.0f;
    private float _verticalVelocity;
    private float _currentSpeed;
    private float _previousBlend;
    private float _previousSpeed = 0;
    private float _smoothness = 2f;
    private float _animBlend;
    private CoroutineRunner _coroutineRunner;
    private Characteristics _characteristics;
    private HealthBar _healthBar;
    private GameObject _afterDeathPanel;
    private Animator _animator;
    private Vector2 _movement;
    private bool _lockPlayerRotation = false;
    private float _fallDistance = 0;
    private bool moveStateRun = true;
    private bool _canMove=true;
    private bool _isStunned=false;
    //Parameters for animator
    private static readonly int RightWalk = Animator.StringToHash("RightWalk");
    private static readonly int StreightWalk = Animator.StringToHash("StraightWalk");
    private static readonly int LeftWalk = Animator.StringToHash("LeftWalk");
    private static readonly int BackWalk = Animator.StringToHash("BackWalk");
    private static readonly int DashDir = Animator.StringToHash("DashDir");
    private static readonly int Dash1 = Animator.StringToHash("Dash");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int IsGetHurt = Animator.StringToHash("IsGetHurt");

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
    private void OnLevelWasLoaded()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level00") return;
        _controller.enabled = false;
        transform.position = new Vector3(0, 26, 0);
        _controller.enabled = true;
    }
    private void Awake()
    {
        _controller = gameObject.GetComponentInChildren<CharacterController>();
        _cameraMain = Camera.main.transform;
    }

    private void Start()
    {
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _healthBar = GameObject.FindWithTag("PlayerHUD").GetComponent<HealthBar>();
        _characteristics = gameObject.GetComponent<Characteristics>();
        Debug.Log(_characteristics);
        _currentStamina = _characteristics.secondCharDic["MaxStamina"];
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _afterDeathPanel = Resources.Load<GameObject>("UI/AfterDeathUICanvas");
        healthSystem.OnDeath += Die;
        healthSystem.OnTakeDamage += TakeDamageAnim;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        if (_isStunned) return;
        JumpAndGravity();
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.G))
        {
            _lockPlayerRotation = !_lockPlayerRotation;
        }
        /*Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter); 
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        { 
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            GameObject hitObject = hit.collider.gameObject;
        }*/
    }
    private void MovePlayer()
    {
        if (!_canMove) return;
        if (_cameraMain == null)
        {
            _cameraMain = Camera.main.transform;
        }
        //Skill
        if (_sprintControl.action.triggered && !skill.isWorking && _currentStamina >= skill.staminaCost && _movement != Vector2.zero)
        {
            skill.Activate(gameObject, _coroutineRunner, _movement);
            _currentStamina -= skill.staminaCost;
            if (_movement.x > 0)
            {
                _animator.SetFloat(DashDir, 1);
            }
            else if (_movement.x < 0)
            {
                _animator.SetFloat(DashDir, 0.75f);
            }
            else if (_movement.y > 0)
            {
                _animator.SetFloat(DashDir, 0);
            }
            else if (_movement.y < 0)
            {
                _animator.SetFloat(DashDir, 0.25f);
            }
            _animator.SetTrigger(Dash1);
        }

        if (_movementControl.action.triggered)
        {
            _movement = _movementControl.action.ReadValue<Vector2>();
        }
        if (_rollControl.action.triggered)
        {
            moveStateRun = !moveStateRun;
        }
        if (_movement != Vector2.zero)
        {
            if (_movement.x > 0 && _movement.y! <= 0)
            {
                _animator.SetBool(RightWalk, true);
                _animator.SetBool(StreightWalk, false);
                _animator.SetBool(LeftWalk, false);
                _animator.SetBool(BackWalk, false);
            }
            else if (_movement.x < 0 && _movement.y! <= 0)
            {
                _animator.SetBool(LeftWalk, true);
                _animator.SetBool(StreightWalk, false);
                _animator.SetBool(RightWalk, false);
                _animator.SetBool(BackWalk, false);
            }
            else if (_movement.y > 0)
            {
                _animator.SetBool(RightWalk, false);
                _animator.SetBool(LeftWalk, false);
                _animator.SetBool(StreightWalk, true);
                _animator.SetBool(BackWalk, false);
            }
            else if (_movement.y < 0)
            {
                _animator.SetBool(BackWalk, true);
                _animator.SetBool(RightWalk, false);
                _animator.SetBool(LeftWalk, false);
                _animator.SetBool(StreightWalk, false);
            }
            if (!moveStateRun && _currentStamina > 0)
            {
                if (_movement.y == 1 || _movement.x != 0)
                {
                    _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.secondCharDic["MovementSpeed"],
                        Time.deltaTime * _smoothness);
                    _animBlend = 0.5f;
                }
                else
                {
                    _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.secondCharDic["MovementSpeed"] / 2,
                        Time.deltaTime * _smoothness);
                    _animBlend = 0.5f;
                }

            }
            else
            {
                if (_movement.y == 1 || _movement.x != 0)
                {
                    _currentSpeed = Mathf.Lerp(_previousSpeed, _characteristics.secondCharDic["MovementSpeed"] + 3f,
                        Time.deltaTime * _smoothness);
                    _animBlend = 1f;
                }
                else
                {
                    _currentSpeed = Mathf.Lerp(_previousSpeed, (_characteristics.secondCharDic["MovementSpeed"] + 3f) / 2,
                        Time.deltaTime * _smoothness);
                    _animBlend = 1f;
                }
            }
            _animator.SetFloat(Speed, Mathf.Lerp(_previousBlend, _animBlend, Time.deltaTime * (_smoothness + 2)));
            _previousBlend = _animator.GetFloat(Speed);
            _previousSpeed = _currentSpeed;

        }
        else
        {
            _animator.SetFloat(Speed, Mathf.Lerp(_previousBlend, 0, Time.deltaTime * (_smoothness + 3)));
            _previousBlend = _animator.GetFloat(Speed);
            _currentSpeed = 0;
        }
       // Debug.Log(GetComponent<Characteristics>().charBuffBuffer["MovementSpeed"]);

        //Stamina

        if (_currentStamina < _characteristics.secondCharDic["MaxStamina"])
        {
            _currentStamina += Time.deltaTime * _characteristics.secondCharDic["StaminaRegen"];
        }
        else if (_currentStamina > _characteristics.secondCharDic["MaxStamina"])
        {
            _currentStamina = _characteristics.secondCharDic["MaxStamina"];
        }
        _healthBar.UpdateStaminaBar(_characteristics.secondCharDic["MaxStamina"], _currentStamina);
        //
        if (!skill.isWorking && (_movement != Vector2.zero || (_movement == Vector2.zero && _lockPlayerRotation)) || !_groundedPlayer)
        {
            Vector3 lookDirection = _cameraMain.forward;
            lookDirection.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            Vector3 move = new Vector3(_movement.x, 0, _movement.y);
            move = _cameraMain.forward * move.z + _cameraMain.right * move.x;
            move.y = 0f;
            _controller.Move(move * (_currentSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }

    public void Die()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        Instantiate(_afterDeathPanel, null);
        Destroy(gameObject);
    }

    private bool IsGrounded()
    {
        float rayDistance = 0.15f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {

                Debug.DrawRay(transform.position, Vector3.down, Color.magenta);
                _animator.SetBool(Grounded, true);
                _fallDistance = 0f;
                _groundedPlayer = true;
                return true;
            }
        }

        _groundedPlayer = false;
        _animator.SetBool(Grounded, false);//

        return false;
    }
    private void JumpAndGravity()
    {
        if (IsGrounded())
        {

            _animator.SetBool(IsFalling, false);
            // stop  velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }
            // Jump
            if (_jumpControl.action.triggered)
            {
                //_animator.SetBool("IsJumping",true);
                _animator.SetTrigger(Jump);
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }
        else
        {
            float rayDistance = 1f;
            bool isFalling = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
            {
                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    Debug.DrawRay(transform.position, Vector3.down, Color.green);
                    isFalling = false;
                }


            }
            if (_fallDistance > 2f && isFalling == true)
            {
                _animator.SetBool(IsFalling, true);
                //_animator.SetBool("IsJumping", false);
            }
            else
            {
                _animator.SetBool(IsFalling, false);
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
        _animator.SetTrigger(IsGetHurt);
    }

    public void AttackAnim(String attackTrigger)
    {
        _animator.SetTrigger(attackTrigger);
    }
    public float GetCurrentStamina()
    {
        return _currentStamina;
    }
    public void DecreaseCurentStamina(float amount)
    {
        _currentStamina -= amount;
    }
    public void ProhibitMoving()
    {
        _canMove = false;
    }
    public void AllowMoving()
    {
        _canMove = true;
    }
    public void Stun()
    {
        _isStunned = true;
        _animator.SetTrigger("Stun");
    }
    public void UnStun()
    {
        _isStunned = false;
        _animator.SetTrigger("UnStun");

    }
}
