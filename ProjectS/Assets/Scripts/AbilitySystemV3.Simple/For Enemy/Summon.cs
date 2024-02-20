using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Summon : Ability
{
    [SerializeField] private GameObject _summonPrefab;
    [SerializeField] private int _spawnAmount;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner, Animator animator)
    {
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(SummonCoroutine(user, animator));
    }

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(SummonCoroutine(user));
    }

    IEnumerator SummonCoroutine(GameObject user, Animator animator)
    {

        animator.SetTrigger(animName);
        yield return new WaitForSeconds(animTime);

        for (int i = _spawnAmount; i > 0; i--)
        {
            Vector3 summonPosition = user.transform.position + Random.insideUnitSphere * 5;
            Instantiate(_summonPrefab, summonPosition, Quaternion.identity);
        }

        abilityIsActive = false;
    }

    IEnumerator SummonCoroutine(GameObject user)
    {
        yield return new WaitForSeconds(animTime);
        SummonsManager summonsManager = user.GetComponent<SummonsManager>();
        for (int i = _spawnAmount; i > 0; i--)
        {
            Vector3 summonPosition = user.transform.position + Random.insideUnitSphere * 5;
            summonsManager.AddSummon(Instantiate(_summonPrefab, summonPosition, Quaternion.identity));;
            
        }

        abilityIsActive = false;
    }
}
