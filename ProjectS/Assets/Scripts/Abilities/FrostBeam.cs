using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBeam : MonoBehaviour
{
    private bool _abilityIsActive = false; 
    public void StartFrostBeam(Transform point, float damagePerSecond, float duration, float beamRange, string aim)
    {
        StartCoroutine(FrostBeamCoroutine(point, damagePerSecond, duration, beamRange, aim));
    }
    IEnumerator FrostBeamCoroutine(Transform point,float damagePerSecond,float duration,float beamRange,string aim)
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

                    hit.collider.GetComponent<HealthSystem>().TakeDamage(damagePerSecond);
                   
                    Debug.Log("Hit");
                }
            }
            elapsedTime+= Time.deltaTime;
            if (elapsedTime>=duration)
            {
                _abilityIsActive = false;
                Debug.Log("AbilityExit");
            }
        
            yield return null;
        }
    }
}
