using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public float fov = 60f;
    public float fovChangeSpeed = 2f;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Check if Camera.main is valid before accessing it
        if (Camera.main != null)
        {
            Camera.main.fieldOfView = fov;
        }
        else
        {
            Debug.LogError("No camera tagged as 'MainCamera' found in the scene.");
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            fov += fovChangeSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
        }
        fov = Mathf.Clamp(fov, 30f, 90f);

        // Check if Camera.main is valid before accessing it
        if (Camera.main != null)
        {
            Camera.main.fieldOfView = fov;
        }
        else
        {
            Debug.LogError("No camera tagged as 'MainCamera' found in the scene.");
        }
    }
}