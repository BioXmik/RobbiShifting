using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target; // Объект, вокруг которого будет вращаться камера
    public float rotationSpeed = 0.5f; // Скорость вращения камеры
    public float maxAngleX = 80f; // Максимальный угол вращения камеры по оси X
    public float minAngleX = -80f; // Минимальный угол вращения камеры по оси X

    private bool isRotating = false; // Флаг, указывающий на активность вращения
    private Vector2 rotationOrigin; // Начальная позиция касания пальца

    private void Update()
    {
        // Проверяем наличие касания на экране
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Запоминаем начальную позицию касания пальца
                    rotationOrigin = touch.position;
                    isRotating = true;
                    break;

                case TouchPhase.Moved:
                    // Если палец двигается по экрану, вращаем камеру вокруг объекта
                    if (isRotating)
                    {
                        Vector2 currentRotation = touch.position - rotationOrigin;
                        float rotationAngleX = -currentRotation.y * rotationSpeed;
                        float rotationAngleY = currentRotation.x * rotationSpeed;

                        // Вычисляем новый угол вращения по осям X и Y
                        float newXRotation = transform.eulerAngles.x + rotationAngleX;
                        float newYRotation = transform.eulerAngles.y + rotationAngleY;

                        // Ограничиваем угол вращения по оси X
                        if (newXRotation > 180f)
                        {
                            newXRotation -= 360f;
                        }
                        newXRotation = Mathf.Clamp(newXRotation, minAngleX, maxAngleX);

                        // Применяем новый угол вращения
                        Quaternion newRotation = Quaternion.Euler(newXRotation, newYRotation, 0);
                        transform.position = target.position - newRotation * Vector3.forward * Vector3.Distance(transform.position, target.position);
                        transform.rotation = newRotation;
                    }
                    break;

                case TouchPhase.Ended:
                    // Касание пальца закончилось, останавливаем вращение
                    isRotating = false;
                    break;
            }
        }
    }
}