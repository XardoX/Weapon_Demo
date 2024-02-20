using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam;

    [SerializeField]
    private Transform headOrientation;

    [SerializeField]
    private float sensitivity = 10f;

    Vector2 lookInput, rotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        vcam.transform.parent = null;
    }

    private void LateUpdate()
    {
        rotation.x -= lookInput.y * sensitivity * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        rotation.y += lookInput.x * sensitivity * Time.deltaTime;

        headOrientation.localEulerAngles = new Vector3(rotation.x, 0f, 0f);
        transform.localEulerAngles = new Vector3(0f, rotation.y, 0f);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
