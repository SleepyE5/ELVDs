using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform bike; // Reference to the bike
    public Transform bikeSeat; // Position where the player sits on the bike
    public MonoBehaviour playerMovement; // Generic player movement script
    public BikeController bikeController; // Bike controller script
    public Camera playerCamera; // Reference to the player's camera
    public Camera bikeCamera; // Reference to the bike's camera

    private bool isRiding = false;

    private void Start()
    {
        // Ensure the player camera is active and the bike camera is inactive at the start
        playerCamera.enabled = true;
        bikeCamera.enabled = false;

        // Ensure the bike is inactive at the start
        if (bikeController != null)
        {
            bikeController.isActive = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact
        {
            if (!isRiding && Vector3.Distance(transform.position, bike.position) < 2f)
            {
                MountBike();
            }
            else if (isRiding)
            {
                DismountBike();
            }
        }
    }

    private void MountBike()
    {
        if (bikeController == null || playerMovement == null || bikeSeat == null || playerCamera == null || bikeCamera == null)
        {
            Debug.LogError("Missing references in PlayerInteraction script!");
            return;
        }

        isRiding = true;
        playerMovement.enabled = false; // Disable player movement
        bikeController.isActive = true; // Enable bike control

        // Move player to bike seat position and rotation
        transform.position = bikeSeat.position;
        transform.rotation = bikeSeat.rotation;

        transform.SetParent(bike); // Parent player to bike

        // Switch to bike camera
        playerCamera.enabled = false;
        bikeCamera.enabled = true;
    }

    private void DismountBike()
    {
        if (bikeController == null || playerMovement == null || playerCamera == null || bikeCamera == null)
        {
            Debug.LogError("Missing references in PlayerInteraction script!");
            return;
        }

        isRiding = false;
        playerMovement.enabled = true; // Enable player movement
        bikeController.isActive = false; // Disable bike control
        transform.SetParent(null); // Unparent player from bike
        transform.position = bike.position + bike.forward * 2f; // Move player off the bike

        // Switch back to player camera
        bikeCamera.enabled = false;
        playerCamera.enabled = true;
    }
    
}