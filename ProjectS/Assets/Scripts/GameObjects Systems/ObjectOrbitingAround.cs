
using UnityEngine;

public class ObjectOrbitingAround : MonoBehaviour
{
    public float orbitRadius = 1.0f;   // ������ ������
    public float orbitSpeed = 50.0f;   // �������� �������� (�������� � �������)
    public float orbitHeight = 0.5f;   // �������� �� ��� Y (������ ������)

    private Vector3 initialOffset;

    void Start()
    {
        // ��������� ��������� ��������, ���������� ������� � ������� �� ������ ������
        initialOffset = transform.localPosition.normalized * orbitRadius;
    }

    void Update()
    {
        // ��������� ���� ��������
        float angle = orbitSpeed * Time.deltaTime;

        // ������ �������� �� ������ ����
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        initialOffset = rotation * initialOffset;

        // ������������� ����� ������� � ������ ������ ������
        transform.localPosition = new Vector3(initialOffset.x, orbitHeight, initialOffset.z);
    }
}
