using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerBuildingBehaviour : MonoBehaviour
{
   
    [SerializeField]
    private NavMeshObstacle _obstacle;
    private Animator _animator;
    private HealthSystem _healthSystem;
    public Action<SpawnerBuildingBehaviour> onDestroy;


    private void Start()
    {
       
        _obstacle = GetComponent<NavMeshObstacle>();
        _animator = GetComponentInChildren<Animator>();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDeath += Die;
        _healthSystem.OnTakeDamage += TakeDamageAnim;
        FindObjectOfType<StageStateManager>().UpdateSpawnersList(this);
    }
 
    public  void Die()
    {
        onDestroy?.Invoke(this);
        Destroy(gameObject);
    }
    public  void TakeDamageAnim()
    {
        //_animator.SetTrigger("Take Damage");
    }
    
}
