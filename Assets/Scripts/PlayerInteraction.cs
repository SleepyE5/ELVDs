using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform bike; // Reference to the bike
    public Transform bikeSeat; // Position where the player sits on the bike
    public GameObject playerCamera; // Player's camera
    public GameObject bikeCamera; // Bike's camera
    private bool isRiding = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact
        {
            if (isRiding)
            {
                DismountBike();
            }
            else if (Vector3.Distance(transform.position, bike.position) < 2f) // Check proximity to bike
            {
                MountBike();
            }
        }
    }

    private void MountBike()
    {
        isRiding = true;
        transform.position = bikeSeat.position; // Move player to bike seat
        transform.SetParent(bike); // Parent player to bike
        playerCamera.SetActive(false); // Disable player camera
        bikeCamera.SetActive(true); // Enable bike camera
        GetComponent<PlayerMovement>().enabled = false; // Disable player movement
        bike.GetComponent<BikeController>().enabled = true; // Enable bike controls
    }

    private void DismountBike()
    {
        isRiding = false;
        transform.SetParent(null); // Unparent player from bike
        playerCamera.SetActive(true); // Enable player camera
        bikeCamera.SetActive(false); // Disable bike camera
        GetComponent<PlayerMovement>().enabled = true; // Enable player movement
        bike.GetComponent<BikeController>().enabled = false; // Disable bike controls
    }
}