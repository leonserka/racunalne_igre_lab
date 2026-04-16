using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleEntry : MonoBehaviour
{
    private bool playerNearby = false;
    private bool isInCar = false;

    private GameObject playerGO;
    private PlayerMovement playerMovement;
    private Renderer playerRenderer;
    private CameraFollow cameraFollow;
    private CarController carController;

    private InputAction enterAction;

    void Awake()
    {
        carController = GetComponent<CarController>();

        var map = new InputActionMap("VehicleMap");
        enterAction = map.AddAction("EnterExit", binding: "<Keyboard>/e");
        enterAction.performed += ctx => HandleEnterExit();
        map.Enable();
    }

    void HandleEnterExit()
    {
        if (isInCar)
            ExitCar();
        else if (playerNearby)
            EnterCar();
    }

    void EnterCar()
    {
        playerMovement.enabled = false;
        playerRenderer.enabled = false;

        carController.isDriving = true;

        cameraFollow.target = transform;
        cameraFollow.offset = new Vector3(0f, 5f, -8f);
        cameraFollow.pitchAngle = 25f;

        isInCar = true;
        GameManager.Instance?.ShowStatus("In car. Press E to exit.", 2f);
    }

    void ExitCar()
    {
        playerGO.transform.position = transform.position + transform.right * 2.5f + Vector3.up * 1f;

        playerRenderer.enabled = true;
        playerMovement.enabled = true;

        carController.isDriving = false;

        cameraFollow.target = playerGO.transform;
        cameraFollow.offset = new Vector3(0f, 7f, -7f);
        cameraFollow.pitchAngle = 40f;

        isInCar = false;
        GameManager.Instance?.ShowStatus("Exited car.", 1.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerGO = other.gameObject;
        playerMovement = other.GetComponent<PlayerMovement>();
        playerRenderer = other.GetComponent<Renderer>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        playerNearby = true;

        if (!isInCar)
            GameManager.Instance?.ShowStatus("Press E to enter car", 99f);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = false;

        if (!isInCar)
            GameManager.Instance?.ShowStatus("", 0f);
    }
}
