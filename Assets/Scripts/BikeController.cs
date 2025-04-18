using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Move the bike forward/backward
        Vector3 moveDirection = transform.forward * forwardInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // Rotate the bike
        float turn = turnInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}