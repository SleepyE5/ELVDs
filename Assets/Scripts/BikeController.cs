using UnityEngine;

public class BikeController : MonoBehaviour
{
    [Header("Bike Settings")]
    public float maxMoveSpeed = 20f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float turnSpeed = 50f;
    public float leanAngle = 30f; // Maximum lean angle for turning
    public float balanceSpeedThreshold = 2f; // Speed below which the bike "wobbles"
    public LayerMask groundLayer;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool isGrounded = true;
    public bool isActive = false;

    private Transform player; // Reference to the player
    private bool canMoveForward = true; // Flag to control forward movement

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        // Check if the bike is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        if (!isGrounded) return;

        float forwardInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Handle forward/backward movement
        if (forwardInput > 0 && canMoveForward)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, forwardInput * maxMoveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else if (forwardInput < 0) // Allow backward movement
        {
            currentSpeed = Mathf.Lerp(currentSpeed, forwardInput * maxMoveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        // Apply forward movement
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Handle turning and leaning
        float targetLean = -turnInput * leanAngle * Mathf.Clamp01(currentSpeed / maxMoveSpeed);
        float currentLean = transform.localRotation.eulerAngles.z;
        if (currentLean > 180f) currentLean -= 360f; // Normalize angle

        // Use Mathf.SmoothDamp for smoother transitions
        float leanVelocity = 0f; // Velocity tracker for SmoothDamp
        float smoothedLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, 0.2f); // Adjust the smoothing time (0.2f) as needed
        transform.localRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y, smoothedLean);

        // Scale turning based on lean angle
        float leanFactor = Mathf.Abs(smoothedLean) / leanAngle; // Normalize lean angle (0 to 1)
        if (Mathf.Abs(currentSpeed) > 0.01f && turnInput != 0)
        {
            float turnAmount = turnInput * turnSpeed * leanFactor * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    public void MountBike(Transform playerTransform)
    {
        player = playerTransform;
        player.gameObject.SetActive(false); // Disable player movement
        player.position = transform.position; // Position player on the bike
        isActive = true; // Activate the bike
    }

    public void DismountBike()
    {
        if (player != null)
        {
            player.gameObject.SetActive(true); // Enable player movement
            player.position = transform.position + Vector3.right; // Place player next to the bike
            player = null;
        }
        isActive = false; // Deactivate the bike
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bike hits a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            canMoveForward = false; // Disable forward movement
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Re-enable forward movement when no longer colliding with a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            canMoveForward = true;
        }
    }
}