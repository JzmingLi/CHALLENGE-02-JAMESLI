using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] InputActionAsset input;
    [SerializeField] Transform playerOrientation;
    [SerializeField] float sensitivity;
    InputAction LookAction;
    float rotationX;
    float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        LookAction = input.FindActionMap("Player").FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerOrientation.position + new Vector3(0,0.35f,0);
        AimCamera();
    }

    void AimCamera()
    {
        Vector2 mouseInput = LookAction.ReadValue<Vector2>() * sensitivity;
        rotationX -= mouseInput.y;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        rotationY += mouseInput.x;
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        playerOrientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
