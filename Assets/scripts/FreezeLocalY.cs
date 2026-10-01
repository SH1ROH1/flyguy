using UnityEngine;

public class FreezeLocalY : MonoBehaviour
{
    // Сюда перетащите вашу камеру (Main Camera) в инспекторе
    [SerializeField] private Transform targetCamera;

    private Vector3 offset;
    private float initialWorldY;

    void Start()
    {
        if (targetCamera != null)
        {
            // 1. Запоминаем стартовую разницу в позициях (дистанцию) между объектом и камерой
            offset = transform.position - targetCamera.position;
        }

        // 2. Запоминаем начальную высоту объекта, чтобы она никогда не менялась
        initialWorldY = transform.position.y;
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            // 3. Вычисляем новую позицию: позиция камеры + наше стартовое смещение
            Vector3 targetPosition = targetCamera.position + offset;

            // 4. Перезаписываем X и Z, а Y оставляем «законстанченным»
            transform.position = new Vector3(targetPosition.x, initialWorldY, targetPosition.z);
        }
    }
}
