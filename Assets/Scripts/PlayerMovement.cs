using UnityEngine;

public class NarrativeFirstPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float fastWalkSpeed = 3f;
    public float rotationSpeed = 2f;
    public float groundDrag = 5f;
    public float movementSmoothing = 0.1f;

    [Header("References")]
    public Transform playerCamera;
    public float cameraSensitivity = 100f;
    public LayerMask groundLayer;

    [Header("Tracking Metrics")]
    public float currentSpeed; // Current speed of the player
    public float totalDistanceTraveled; // Total distance traveled
    public float timeSpentMoving; // Time spent moving

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    private Vector3 currentVelocity;
    private Vector3 lastPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;

        lastPosition = transform.position; // Initialize last position
    }

    private void Update()
    {
        GetInput();
        RotateCamera();
        ApplyDrag();

        // Adjust movement speed based on Shift key
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? fastWalkSpeed : 3f;

        // Update tracking metrics
        currentSpeed = rb.velocity.magnitude; // Calculate current speed
        if (currentSpeed > 0.1f) // Consider movement only if speed is significant
        {
            totalDistanceTraveled += Vector3.Distance(transform.position, lastPosition);
            timeSpentMoving += Time.deltaTime;
        }
        lastPosition = transform.position; // Update last position
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 targetVelocity = moveDirection.normalized * moveSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, movementSmoothing);
        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }

    private void RotateCamera()
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Quaternion targetRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerCamera.localRotation = Quaternion.Slerp(playerCamera.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void ApplyDrag()
    {
        if (horizontalInput == 0 && verticalInput == 0)
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0f, rb.velocity.y, 0f), groundDrag * Time.deltaTime);
        }
    }
}