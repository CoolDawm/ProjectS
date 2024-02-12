using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] protected List<Ability> abilityList;
    [HideInInspector] public float[] timers = new float[4] { 0, 0, 0, 0 };
    private Characteristics _characteristics;
    private CoroutineRunner _coroutineRunner;
    private float _currentMana;
    private PlayerBehaviour _playerBehaviour;
    private HealthBar _healthBar;
    private Animator _animator;
    [SerializeField]
    private GameObject aim;
    private static readonly int IsGetHurt = Animator.StringToHash("IsGetHurt");
    //private bool _isActive=false;// for future improvement(to not be able to use other abilities)
    
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
        _healthBar=GameObject.FindWithTag("PlayerHUD").GetComponent<HealthBar>();
    }

    public void GenerateMana(float mana)
    {
        if (_currentMana+mana >= _characteristics.secondCharDic["MaxMana"])
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

        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i])
            {
                
            }
        }

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            aim.transform.position = targetPosition;
            Debug.DrawRay(targetPosition,Vector3.up, Color.red);
        }
        
        if (Input.GetMouseButtonDown(0) && timers[0] <= 0)
        {
            if (!abilityList[0].abilityIsActive)
            {
                StartCoroutine(ActivateAbility(0));
                timers[0] = abilityList[0].cooldown;
                _animator.ResetTrigger(IsGetHurt);
            }
            
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
        _healthBar.UpdateManaBar(_characteristics.secondCharDic["MaxMana"],_currentMana);
    }

    private void RotateToAttack()
    {
        Transform _cameraMain = Camera.main.transform;
        Vector3 lookDirection = _cameraMain.forward;
        lookDirection.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,5); 
    }
    
    IEnumerator ActivateAbility(int index)
    {
        _playerBehaviour.AttackAnim(abilityList[index].animName);
        RotateToAttack();
        yield return new WaitForSeconds(abilityList[index].animTime);
        abilityList[index].Activate(gameObject, _coroutineRunner);
    }
}