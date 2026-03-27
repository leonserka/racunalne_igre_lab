using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 5f;
    public float gravity = -20f;

    [Header("Crouch")]
    public float standHeight = 2f;
    public float crouchHeight = 1f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;
    public float staminaThreshold = 20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraHolder;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    // Private variables
    private CharacterController cc;
    private Vector3 velocity;
    public float currentStamina;
    private float pitch = 0f;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool isGrounded = false;

    // Player states
    public enum PlayerState { Idle, Walk, Run, Crouch, Jump, Fall }
    public PlayerState currentState = PlayerState.Idle;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleStamina();
        UpdateState();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0f, mouseX, 0f);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraHolder.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Čučanj
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            cc.height = isCrouching ? crouchHeight : standHeight;
        }

        // Trčanje
        isRunning = Input.GetKey(KeyCode.LeftShift)
                    && currentStamina > 0
                    && !isCrouching;

        // Odabir brzine
        float speed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);

        // Kretanje
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        cc.Move(move * speed * Time.deltaTime);

        // Provjera tla pomoću raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down,
                                      cc.height / 2f + 0.1f);

        // Gravitacija
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Skok
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    void HandleStamina()
    {
        if (isRunning)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);

            if (currentStamina <= staminaThreshold)
                isRunning = false;
        }
        else
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    void UpdateState()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        if (!isGrounded && velocity.y < 0)
            currentState = PlayerState.Fall;
        else if (!isGrounded && velocity.y > 0)
            currentState = PlayerState.Jump;
        else if (isCrouching)
            currentState = PlayerState.Crouch;
        else if (isMoving && isRunning)
            currentState = PlayerState.Run;
        else if (isMoving)
            currentState = PlayerState.Walk;
        else
            currentState = PlayerState.Idle;
    }
}