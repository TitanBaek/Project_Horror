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
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;  // 메인카메라의 LookDistance 의 앞을 lookPoint로 
        aimTarget.position = lookPoint;                   // aimTarget을 해당 위치에 배치
        lookPoint.y = transform.position.y;               // lookPoint의 y를 현재 플레이어의 y로 
        transform.LookAt(lookPoint);                      // 플레이어가 해당 위치를 바라보게 끔 LookAt <- 이부분이 문제다 지금
    }
        
    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime; // 마우스로 카메라 회전 구현
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime; // 마우스로 카메라 회전 구현
        xRotation = Mathf.Clamp(xRotation, -rocationDistance, rocationDistance);                // 시야 회전 각도 제한

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
