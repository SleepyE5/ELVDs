using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Sensitivity of the mouse movement
    public float mouseSensitivity = 100f;
    // Reference to the player's body transform
    public Transform playerBody;
    // Initial field of view (FOV) of the camera
    public float fov = 60f;
    // Speed at which the FOV changes
    public float fovChangeSpeed = 2f;

    // Variable to store the vertical rotation of the camera
    private float xRotation = 0f;

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // Set the initial FOV of the camera
        Camera.main.fieldOfView = fov;
    }

    void Update()
    {
        // Get mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the vertical rotation based on mouse Y movement
        xRotation -= mouseY;
        // Clamp the vertical rotation to prevent flipping
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotate the player body based on mouse X movement
        playerBody.Rotate(Vector3.up * mouseX);

        // Change the FOV using the up and down arrow keys
        if (Input.GetKey(KeyCode.UpArrow))
        {
            fov += fovChangeSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            fov -= fovChangeSpeed * Time.deltaTime;
        }
        // Clamp the FOV to a range between 30 and 90
        fov = Mathf.Clamp(fov, 30f, 90f);
        // Apply the FOV change to the camera
        Camera.main.fieldOfView = fov;
    }
}