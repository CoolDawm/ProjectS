using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
public class Dash : Skill
{
    public float dashSpeed = 10f;
    public float dashDistance = 0.5f;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner,Vector2 direction)
    {
        coroutineRunner.StartCoroutineFunction(Dashing(user,direction));
    }
    private IEnumerator Dashing(GameObject user, Vector2 direction)
    {
        isWorking = true;
        CharacterController controller = user.GetComponent<CharacterController>();
        Vector3 startPos = user.transform.position;
        Vector3 dashDir = new Vector3(direction.x, 0, direction.y).normalized; // Используем direction для задания направления
        dashDir = user.transform.TransformDirection(dashDir); // Преобразуем вектор направления в локальные координаты
        float remainingDistance = dashDistance;
        float currentSpeed = dashSpeed;
        while (remainingDistance > 0)
        {
            float moveDistance = currentSpeed * Time.deltaTime;
            if (moveDistance > remainingDistance)
            {
                moveDistance = remainingDistance;
            }
            controller.Move(dashDir * moveDistance);
            remainingDistance -= moveDistance;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        isWorking = false;
    }
}
/*
 * private IEnumerator Dashing(GameObject user,Vector2 direction)
    {
        isWorking = true;
        CharacterController controller = user.GetComponent<CharacterController>();
        Vector3 startPos = user.transform.position;
        Vector3 dashDir = new Vector3(direction.x,0,direction.y);
        float remainingDistance = dashDistance;
        float currentSpeed = dashSpeed;
        while (remainingDistance > 0)
        {
            float moveDistance = currentSpeed * Time.deltaTime;
            if (moveDistance > remainingDistance)
            {
                moveDistance = remainingDistance;
            }
            controller.Move(dashDir * moveDistance);
            remainingDistance -= moveDistance;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        isWorking = false;
    }
 */