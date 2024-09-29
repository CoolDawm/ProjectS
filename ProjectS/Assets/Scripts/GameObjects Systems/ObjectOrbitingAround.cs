
using UnityEngine;

public class ObjectOrbitingAround : MonoBehaviour
{
    public float orbitRadius = 1.0f;   // Радиус орбиты
    public float orbitSpeed = 50.0f;   // Скорость вращения (градусов в секунду)
    public float orbitHeight = 0.5f;   // Смещение по оси Y (высота орбиты)

    private Vector3 initialOffset;

    void Start()
    {
        // Вычисляем начальное смещение, нормализуя позицию и умножая на радиус орбиты
        initialOffset = transform.localPosition.normalized * orbitRadius;
    }

    void Update()
    {
        // Вычисляем угол поворота
        float angle = orbitSpeed * Time.deltaTime;

        // Создаём вращение на основе угла
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        initialOffset = rotation * initialOffset;

        // Устанавливаем новую позицию с учетом высоты орбиты
        transform.localPosition = new Vector3(initialOffset.x, orbitHeight, initialOffset.z);
    }
}
