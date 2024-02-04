using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MeleeBoss : BossBehaviour
{
    [SerializeField]
    private float _attackCooldown = 0;
    private NavMeshAgent _agent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _attackRange = 15f;
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>();
        //_characteristics.charBuffBuffer["MovementSpeed"] = 10f;
        healthSystem.OnDeath += Die;
        healthSystem.OnTakeDamage+=TakeDamageAnim;
    }

    protected override void FixedUpdate()
    {
        if (_player == null)
        {
            return;
        }
        if (_agent.hasPath)
        {
            _animator.SetBool("Move Forward Fast", true);
        }
        else
        {
            _animator.SetBool("Move Forward Fast", false);

        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius||isAggro)
        {

            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                Idle();
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                _abilityCooldown += Time.deltaTime;
                
                if (_attackCooldown >= 5f)
                {
                    Vector3 targetDirection = _player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
                    _attackCooldown = 0;
                    Attack();
                }

                if (_abilityCooldown >= 8f)
                {
                    Vector3 targetDirection = _player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
                    _abilityCooldown = 0;
                    int randomValue = UnityEngine.Random.Range(1, abilityList.Count-1);
                    UseAbility(randomValue);
                }
            }
            else
            {
                ChasePlayer();
            }
        }
    }
   
    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_player.transform.position);
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }
    
    public override void TakeDamageAnim()
    {
        isAggro = true;
        _animator.SetTrigger("Take Damage");
    }
    public override void UseAbility(int index)
    {
        _animator.SetTrigger(abilityList[index].animName);
        abilityList[index].Activate(gameObject,_coroutineRunner);
    }
    /*
    IEnumerator ActivateAbility(int index)
    {
        _animator.SetTrigger(abilityList[index].animName);
        Debug.Log("Activate");
        yield return new WaitForSeconds(abilityList[index].animTime);
        Debug.Log("End");
        abilityList[index].Activate(gameObject, _coroutineRunner);

        
         float timer = 0;
        _animator.SetTrigger(abilityList[index].animName);
        while (timer<=abilityList[index].animTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        abilityList[index].Activate(gameObject,_coroutineRunner);
        Debug.Log("Attack");
    }*/
    public override void Attack()
    {
        _animator.SetTrigger(abilityList[0].animName);
        abilityList[0].Activate(gameObject,_coroutineRunner);
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
