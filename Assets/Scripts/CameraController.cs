using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam;

    [SerializeField]
    private float sensitivity = 10f;

    private Camera mainCamera;

    Vector2 lookInput, rotation;

    private void Awake()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        rotation.x -= lookInput.y * sensitivity * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        rotation.y += lookInput.x * sensitivity * Time.deltaTime;

        vcam.transform.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        //transform.Rotate(Vector3.up, rotation.y);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
