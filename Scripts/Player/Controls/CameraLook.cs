using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownRange = 80.0f;

    private float verticalRotation = 0.0f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // horizontal
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        playerTransform.Rotate(Vector3.up * mouseXRotation);

        // vertical
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
    }
}