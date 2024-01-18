using Invector.vCharacterController;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target, old, cam;
    public float speed, turnSpeed;
    public LayerMask maskObstacles;

    private Vector2 _previousTouchPosition;

    public float rotationSpeedMobile = 0.05f; // �������� �������� ������
    public float mobileSmooth;
    public float rotationSpeed = 2f; // �������� �������� ������
    public float maxAngleX = 80f; // ������������ ���� �������� ������ �� ��� X
    public float minAngleX = -80f; // ����������� ���� �������� ������ �� ��� X

    private bool isRotating = false; // ����, ����������� �� ���������� ��������
    private Vector2 rotationOrigin; // ��������� ������� ������� ������

    private PlayerController playerController;
    public bool mobileVersion;

    private float mouseX, mouseY;
    private Vector3 lastMousePosition;

    public bool lockCursor = true;

    private Vector3 offset;
    private float X, Y;

    private Quaternion newRotation;
    private Vector3 thisCameraPosition;
    private Quaternion thisCameraRotation;
    private vThirdPersonController cc;

    void Start()
    {
        SetRadius(4);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        mobileVersion = GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonInput>().mobileVersion;
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonController>();
    }

    private void Update()
    {
        if (mobileVersion)
        {
            RoationMobile();
        }
        else
        {
            RotationPC();
        }
    }

    private void RoationMobile()
    {
        // ��������� ������� ������� �� ������
        if (Input.touchCount > 0)
        {
            // ������������ ������ �������
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x > Screen.width / 2)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            // ���������� ��������� ������� ������� ������
                            rotationOrigin = touch.position;
                            isRotating = true;
                            break;

                        case TouchPhase.Moved:
                            // ���� ����� ��������� �� ������, ������� ������ ������ �������
                            Vector2 currentRotation = touch.position - rotationOrigin;
                            float rotationAngleX = -currentRotation.y * rotationSpeedMobile;
                            float rotationAngleY = currentRotation.x * rotationSpeedMobile;

                            // ��������� ����� ���� �������� �� ���� X � Y
                            float newXRotation = transform.eulerAngles.x + rotationAngleX;
                            float newYRotation = transform.eulerAngles.y + rotationAngleY;

                            if (isRotating)
                            {
                                // ������������ ���� �������� �� ��� X
                                if (newXRotation > 180f)
                                {
                                    newXRotation -= 360f;
                                }
                                newXRotation = Mathf.Clamp(newXRotation, minAngleX, maxAngleX);

                                // ��������� ����� ���� ��������
                                newRotation = Quaternion.Euler(newXRotation, newYRotation, 0);
                                thisCameraPosition = target.position - newRotation * Vector3.forward * Vector3.Distance(transform.position, target.position);
                                thisCameraRotation = newRotation;
                            }
                            break;

                        case TouchPhase.Ended:
                            // ������� ������ �����������, ������������� ��������
                            isRotating = false;
                            break;
                    }
                }
                else
                {
                    thisCameraPosition = target.position - newRotation * Vector3.forward * Vector3.Distance(transform.position, target.position);
                }
            }
        }
        if (cc.isJumping || !cc.isGrounded)
        {
            thisCameraPosition = target.position - newRotation * Vector3.forward * Vector3.Distance(transform.position, target.position);
            thisCameraRotation = newRotation;
        }
        transform.position = Vector3.Lerp(transform.position, thisCameraPosition, Time.deltaTime * mobileSmooth);
        transform.rotation = Quaternion.Lerp(transform.rotation, thisCameraRotation, Time.deltaTime * mobileSmooth);
    }

    private void RotationPC()
    {
        X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed;
        Y += Input.GetAxis("Mouse Y") * rotationSpeed;
        Y = Mathf.Clamp(Y, -maxAngleX, maxAngleX);
        transform.localEulerAngles = new Vector3(-Y, X, 0);
        transform.position = transform.localRotation * offset + target.position;
    }

    public void SetRadius(float value)
    {
        offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(value), -Mathf.Abs(value));
        transform.position = transform.localRotation * offset + target.position;
    }

    void FixedUpdate()
    {
        float xAngle = 0;
        if (transform.localEulerAngles.x > 90 || transform.localEulerAngles.x < 2.5f)
        {
            xAngle = 0.2f;
        }
        else
        {
            xAngle = -0.2f;
        }

        RaycastHit hit;
        if (Physics.Raycast(target.position, old.position - target.position, out hit, Vector3.Distance(old.position, target.position), maskObstacles))
        {
            cam.position = hit.point + new Vector3(0, xAngle, 0);
        }
        else
        {
            cam.position = old.position + new Vector3(0, xAngle, 0);
        }
    }
}