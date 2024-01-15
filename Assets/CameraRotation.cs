using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target; // ������, ������ �������� ����� ��������� ������
    public float rotationSpeed = 0.5f; // �������� �������� ������
    public float maxAngleX = 80f; // ������������ ���� �������� ������ �� ��� X
    public float minAngleX = -80f; // ����������� ���� �������� ������ �� ��� X

    private bool isRotating = false; // ����, ����������� �� ���������� ��������
    private Vector2 rotationOrigin; // ��������� ������� ������� ������

    private void Update()
    {
        // ��������� ������� ������� �� ������
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // ���������� ��������� ������� ������� ������
                    rotationOrigin = touch.position;
                    isRotating = true;
                    break;

                case TouchPhase.Moved:
                    // ���� ����� ��������� �� ������, ������� ������ ������ �������
                    if (isRotating)
                    {
                        Vector2 currentRotation = touch.position - rotationOrigin;
                        float rotationAngleX = -currentRotation.y * rotationSpeed;
                        float rotationAngleY = currentRotation.x * rotationSpeed;

                        // ��������� ����� ���� �������� �� ���� X � Y
                        float newXRotation = transform.eulerAngles.x + rotationAngleX;
                        float newYRotation = transform.eulerAngles.y + rotationAngleY;

                        // ������������ ���� �������� �� ��� X
                        if (newXRotation > 180f)
                        {
                            newXRotation -= 360f;
                        }
                        newXRotation = Mathf.Clamp(newXRotation, minAngleX, maxAngleX);

                        // ��������� ����� ���� ��������
                        Quaternion newRotation = Quaternion.Euler(newXRotation, newYRotation, 0);
                        transform.position = target.position - newRotation * Vector3.forward * Vector3.Distance(transform.position, target.position);
                        transform.rotation = newRotation;
                    }
                    break;

                case TouchPhase.Ended:
                    // ������� ������ �����������, ������������� ��������
                    isRotating = false;
                    break;
            }
        }
    }
}