using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class SimpleRangeV2 : Ability
{
    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private float _projectileSpeed = 100f;
    [SerializeField]
    private float _rangeRadius = 25f; // ������ ���� ����� �������
    [SerializeField]
    private float _fieldOfViewAngle = 45f; // ���� ������ ����� �������
    private GameObject _shootingPosition;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        if (_shootingPosition == null)
        {
            _shootingPosition = user.GetComponentsInChildren<Transform>()
                .FirstOrDefault(c => c.gameObject.name == "ShootingPosition")?.gameObject;
        }

        // ���� ���������� ����� � ���� ����� �������
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, _rangeRadius);
        Collider closestCollider = null;
        float closestDistance = _rangeRadius;
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget < closestDistance && Vector3.Angle(user.transform.forward, directionToTarget) < _fieldOfViewAngle / 2)
            {
                if (collider.transform.root.CompareTag("Enemy")) // �����������, ��� ����� ����� ��� "Enemy"
                {
                    closestCollider = collider;
                    closestDistance = distanceToTarget;
                }
            }
        }

        // ������� ������
        GameObject projectile = Instantiate(_projectilePrefab, _shootingPosition.transform.position, Quaternion.identity);
        ProjectileScript prScr = projectile.GetComponentInChildren<ProjectileScript>();

        if (closestCollider != null)
        {
            // ���� �������
            user.GetComponent<AbilityHolder>().GenerateMana(3); // ��������� ���� ��� ���������� ����, ���� ��� �����
            prScr.aim = "Enemy"; // ������������� ��� ����
            prScr.range = _rangeRadius;
            Vector3 direction = (closestCollider.transform.position - _shootingPosition.transform.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.transform.Rotate(90, 0, 0);
            projectile.GetComponentInChildren<Rigidbody>().velocity = direction * _projectileSpeed;
        }
        else
        {
            // ���� �� �������, �������� ����� ����� �����
            prScr.aim = "Enemy"; // ������������� ��� ����
            prScr.range = _rangeRadius;
            Vector3 direction = user.transform.forward;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.transform.Rotate(90, 0, 0);
            projectile.GetComponentInChildren<Rigidbody>().velocity = direction * _projectileSpeed;
        }
    }
}
