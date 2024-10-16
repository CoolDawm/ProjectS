using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesHolder : MonoBehaviour
{
    [SerializeField] protected List<Ability> abilityList;
    [HideInInspector] public float[] timers = new float[4] { 0, 0, 0, 0 };
    private Characteristics _characteristics;
    private CoroutineRunner _coroutineRunner;
    private float _currentMana;
    private PlayerBehaviour _playerBehaviour;
    private HealthBar _healthBar;
    private Animator _animator;
    private static readonly int IsGetHurt = Animator.StringToHash("IsGetHurt");

    private bool isStrongAttack = false;
    private bool isMouseButtonDown = false;
    private bool isStaminaEnough = true;
    private float holdTimeThreshold = 1f;
    private float mouseDownTime = 0f;
    private float lastStrongAttackTime = 0f;
    private float strongAttackCooldown = 1f;

    private void Start()
    {
        _characteristics = GetComponent<Characteristics>();
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _currentMana = _characteristics.secondCharDic["MaxMana"];
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _animator = GetComponent<Animator>();
        for (int i = 0; i < abilityList.Count; i++)
        {
            Debug.Log(abilityList[i].name);
        }
        _healthBar = GameObject.FindWithTag("PlayerHUD").GetComponent<HealthBar>();
    }

    public void GenerateMana(float mana)
    {
        if (_currentMana + mana >= _characteristics.secondCharDic["MaxMana"])
        {
            _currentMana = _characteristics.secondCharDic["MaxMana"];
        }
        else
        {
            _currentMana += mana;
        }
    }

    public List<Ability> GetAbilitiesList()
    {
        return abilityList;
    }

    public void ChangeAbility(Ability ability, int place)
    {
        abilityList[place] = ability;
        for (int i = 0; i < abilityList.Count; i++)
        {
            Debug.Log(abilityList[i].name);
        }
    }

    private void Update()
    {
        if (Cursor.visible)
        {
            return;
        }

        HandleAttackInputs();
        UpdateManaAndCooldowns();
        UpdateHealthBar();
    }

    private void HandleAttackInputs()
    {
        float holdDuration;
        if (Input.GetMouseButtonDown(0))
        {
            if (!isMouseButtonDown)
            {
                isMouseButtonDown = true;
                mouseDownTime = Time.time; // button down last time
            }
        }

        if (Input.GetMouseButton(0))
        {
            holdDuration = Time.time - mouseDownTime;

            if (holdDuration >= holdTimeThreshold && Time.time - lastStrongAttackTime >= strongAttackCooldown)
            {
                if (_playerBehaviour.GetCurrentStamina() >= abilityList[0].strongAttackStaminaCost && isStaminaEnough)
                {
                    _playerBehaviour.DecreaseCurentStamina(abilityList[0].strongAttackStaminaCost);
                    StartCoroutine(PerformStrongAttack());
                    lastStrongAttackTime = Time.time; // last strong attack time
                    mouseDownTime = Time.time; // For correct work with strong attack
                    timers[0] = abilityList[0].cooldown;

                    if (_playerBehaviour.GetCurrentStamina() < abilityList[0].strongAttackStaminaCost)
                    {
                        isStaminaEnough = false; // Stamina is not enough for the next strong attack
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            holdDuration = Time.time - mouseDownTime;

            if (holdDuration <= holdTimeThreshold && timers[0] <= 0)
            {
                PerformComboAttack();
                timers[0] = abilityList[0].cooldown;
            }
            isMouseButtonDown = false;
            isStaminaEnough = true; // Reset stamina flag on button release
        }

        if (Input.GetMouseButtonDown(1) && _currentMana >= abilityList[1].manaCost && timers[1] <= 0)
        {
            _currentMana -= abilityList[1].manaCost;
            StartCoroutine(ActivateAbility(1));
            timers[1] = abilityList[1].cooldown;
            _animator.ResetTrigger(IsGetHurt);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && _currentMana >= abilityList[2].manaCost && timers[2] <= 0)
        {
            _currentMana -= abilityList[2].manaCost;
            StartCoroutine(ActivateAbility(2));
            timers[2] = abilityList[2].cooldown;
            _animator.ResetTrigger(IsGetHurt);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && _currentMana >= abilityList[3].manaCost && timers[3] <= 0)
        {
            _currentMana -= abilityList[3].manaCost;
            StartCoroutine(ActivateAbility(3));
            timers[3] = abilityList[3].cooldown;
            _animator.ResetTrigger(IsGetHurt);
        }
    }

    private void PerformComboAttack()
    {
        Debug.Log("PerformComboAttack Started");
        if (!abilityList[0].abilityIsActive)
        {
            StartCoroutine(ActivateAbility(0));
            _animator.ResetTrigger(IsGetHurt);
        }
    }

    private IEnumerator PerformStrongAttack()
    {
        Debug.Log("PerformStrongAttack Started");

        if (!abilityList[0].abilityIsActive)
        {
            _playerBehaviour.AttackAnim(abilityList[0].strongAttackName);
            RotateToAttack();
            yield return new WaitForSeconds(abilityList[0].strongAttackTime);
            yield return _coroutineRunner.StartCoroutine(abilityList[0].ActivateStrongAttack(gameObject, _coroutineRunner));
            _animator.ResetTrigger(IsGetHurt);
        }
    }

    private void UpdateManaAndCooldowns()
    {
        for (int i = 0; i < timers.Length; i++)
        {
            if (timers[i] > 0)
            {
                timers[i] -= Time.deltaTime;
            }
            else
            {
                timers[i] = 0;
            }
        }
        if (_currentMana > _characteristics.secondCharDic["MaxMana"])
        {
            _currentMana = _characteristics.secondCharDic["MaxMana"];
        }
        else
        {
            _currentMana += Time.deltaTime * _characteristics.secondCharDic["ManaRegen"];
        }
    }

    private void UpdateHealthBar()
    {
        _healthBar.UpdateManaBar(_characteristics.secondCharDic["MaxMana"], _currentMana);
    }

    private void RotateToAttack()
    {
        Transform _cameraMain = Camera.main.transform;
        Vector3 lookDirection = _cameraMain.forward;
        lookDirection.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5);
    }

    IEnumerator ActivateAbility(int index)
    {
        _playerBehaviour.AttackAnim(abilityList[index].animName);
        RotateToAttack();
        if (abilityList[index].animName!="JumpA")
        {
            yield return new WaitForSeconds(abilityList[index].animTime);
            abilityList[index].Activate(gameObject, _coroutineRunner);
            yield break;
        }
        abilityList[index].Activate(gameObject, _coroutineRunner);

    }
}
