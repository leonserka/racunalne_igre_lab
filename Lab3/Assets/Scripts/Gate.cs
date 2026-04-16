using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Gate : MonoBehaviour
{
    [Header("Gate Settings")]
    public float openHeight = 4f;
    public float moveSpeed = 3f;
    public float autoCloseDelay = 5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool playerNearby = false;
    private bool isOpen = false;
    private bool isMoving = false;

    private InputAction interactAction;

    void Awake()
    {
        var map = new InputActionMap("GateMap");
        interactAction = map.AddAction("Interact", binding: "<Keyboard>/e");
        interactAction.performed += ctx => HandleInteract();
        map.Enable();
    }

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void HandleInteract()
    {
        if (!playerNearby || isMoving || isOpen) return;
        StartCoroutine(OpenAndAutoClose());
    }

    private IEnumerator OpenAndAutoClose()
    {
        isMoving = true;
        GameManager.Instance?.ShowStatus("Gate opening...", 1f);

        yield return StartCoroutine(MoveGate(closedPosition, openPosition));

        isOpen = true;
        isMoving = false;
        GameManager.Instance?.ShowStatus("Gate open! Closes in " + autoCloseDelay + "s", 2f);

        yield return new WaitForSeconds(autoCloseDelay);

        isMoving = true;
        GameManager.Instance?.ShowStatus("Gate closing!", 1f);

        yield return StartCoroutine(MoveGate(openPosition, closedPosition));

        isOpen = false;
        isMoving = false;
    }

    private IEnumerator MoveGate(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = openHeight / moveSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        transform.position = to;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = true;
        GameManager.Instance?.ShowStatus("Press E to open gate", 99f);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = false;
        if (!isOpen && !isMoving)
            GameManager.Instance?.ShowStatus("", 0f);
    }
}
