using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class TotemSummon :  Ability
{
    [SerializeField] private GameObject _summonPrefab;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(SummonCoroutine(user));
    }
    IEnumerator SummonCoroutine(GameObject user)
    {
        yield return new WaitForSeconds(animTime);
        //SummonsManager summonsManager = user.GetComponent<SummonsManager>();
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;
            //Vector3 summonPosition = user.transform.position + Random.insideUnitSphere * 5;
            Instantiate(_summonPrefab, spawnPosition, Quaternion.identity);
            //summonsManager.AddSummon();;
        }
        abilityIsActive = false;
    }
}