using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f; // Скорость движения врага

    [SerializeField]
    private Transform pointA; // Первая точка для движения
    [SerializeField]
    private Transform pointB; // Вторая точка для движения

    private Transform targetPoint; // Текущая цель для перемещения

    private void Start()
    {
        // Устанавливаем начальную цель как точку A
        targetPoint = pointA;
    }

    private void Update()
    {
        // Перемещаем врага в сторону целевой точки
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // Если враг достигает целевой точки, меняем направление
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }
}
