using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
public class Dash : Skill
{
    public float rollSpeed = 10f;
    public float rollDistance = 0.5f;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        coroutineRunner.StartCoroutineFunction(Dashing(user));
    }
    private IEnumerator Dashing(GameObject user)
    {
        isWorking = true;
        Vector3 dashDirection = user.transform.forward;
        float elapsedTime = 0f;
        while (elapsedTime < rollDistance)
        {
            float t = elapsedTime / rollDistance;
            user.GetComponent<CharacterController>().Move(dashDirection * Time.deltaTime * rollSpeed); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(workingTime);
        isWorking = false;
    }
}
