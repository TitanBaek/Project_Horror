using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform camRoot;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float lookDistance;
    [SerializeField] Transform aimTarget;

    private float xRotation;
    private float yRotation;
    private Vector2 lookDelta;
    private float rocationDistance = 20f;

    void Update()
    {
        Rotate();
    }

    void LateUpdate()
    {
        Look();
    }

    private void Rotate()
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;  // ����ī�޶��� LookDistance �� ���� lookPoint�� 
        aimTarget.position = lookPoint;                   // aimTarget�� �ش� ��ġ�� ��ġ
        lookPoint.y = transform.position.y;               // lookPoint�� y�� ���� �÷��̾��� y�� 
        transform.LookAt(lookPoint);                      // �÷��̾ �ش� ��ġ�� �ٶ󺸰� �� LookAt <- �̺κ��� ������ ����
    }
        
    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime; // ���콺�� ī�޶� ȸ�� ����
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime; // ���콺�� ī�޶� ȸ�� ����
        xRotation = Mathf.Clamp(xRotation, -rocationDistance, rocationDistance);                // �þ� ȸ�� ���� ����

        camRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
