using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class FrostBeamV2 : Ability
{
    public float damagePerSecond;
    public float duration;
    public float beamRange;
    private bool _abilityIsActive = false;
    private Transform point;
    public override  void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        if (point == null)
        {
            point = user.transform.Find("ShootingPosition");
        }
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(FrostBeamCoroutine(user)); 
    }
    IEnumerator FrostBeamCoroutine(GameObject user)
    {
        _abilityIsActive = true;
        float elapsedTime = 0f;
        while (_abilityIsActive)
        {
            Ray ray = new Ray(point.position, point.forward);
            RaycastHit hit;

            Debug.DrawRay(point.position, point.forward,Color.magenta);

            if (Physics.Raycast(ray, out hit, beamRange))
            {

                if (hit.collider.CompareTag(aim))
                {
                    hit.collider.GetComponent<HealthSystem>().TakeDamage(damagePerSecond, Color.yellow);
                }
            }
            elapsedTime+= Time.deltaTime;
            if (elapsedTime>=duration)
            {
                _abilityIsActive = false;
            }
            abilityIsActive = false;
            yield return null;
        }
    }
}
