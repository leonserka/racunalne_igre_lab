using UnityEngine;

public class BoostZone : MonoBehaviour
{
    public float speedMultiplier = 2f;

    private PlayerMovement player;
    private CarController car;
    private float originalPlayerSpeed;
    private float originalCarForce;
    private bool playerInside = false;
    private bool carInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMovement>();
            if (player == null) return;
            originalPlayerSpeed = player.GetSpeed();
            player.SetSpeed(originalPlayerSpeed * speedMultiplier);
            playerInside = true;
            GameManager.Instance?.ShowStatus("BOOST ZONE! 2x speed!", 1.5f);
        }
        else if (other.CompareTag("Car"))
        {
            car = other.GetComponentInParent<CarController>();
            if (car == null) return;
            originalCarForce = car.motorForce;
            car.motorForce = originalCarForce * speedMultiplier;
            carInside = true;
            GameManager.Instance?.ShowStatus("CAR BOOST! 2x motor force!", 1.5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerInside)
        {
            player?.SetSpeed(originalPlayerSpeed);
            playerInside = false;
            GameManager.Instance?.ShowStatus("Left boost zone.", 1f);
        }
        else if (other.CompareTag("Car") && carInside)
        {
            if (car != null) car.motorForce = originalCarForce;
            carInside = false;
            GameManager.Instance?.ShowStatus("Left boost zone.", 1f);
        }
    }
}
