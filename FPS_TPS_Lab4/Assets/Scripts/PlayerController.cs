using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;

    [Header("References")]
    public Transform swatModel;

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

    private CharacterController cc;
    private PlayerEffects effects;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private Vector3 velocity;
    public float currentStamina;
    private float pitch = 0f;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool isGrounded = false;

    public enum PlayerState { Idle, Walk, Run, Crouch, Jump, Fall }
    public PlayerState currentState = PlayerState.Idle;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        effects = GetComponent<PlayerEffects>();
        ConfigureInputActions();
    }

    void Start()
    {
        currentStamina = maxStamina;
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
        jumpAction?.Enable();
        crouchAction?.Enable();
        sprintAction?.Enable();
        attackAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        jumpAction?.Disable();
        crouchAction?.Disable();
        sprintAction?.Disable();
        attackAction?.Disable();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleStamina();
        UpdateState();
        UpdateAnimator();
    }

    void ConfigureInputActions()
    {
        moveAction = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        moveAction.AddBinding("<Gamepad>/leftStick");

        lookAction = new InputAction("Look", InputActionType.Value, expectedControlType: "Vector2");
        lookAction.AddBinding("<Pointer>/delta");
        lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("scaleVector2(x=15,y=15)");

        jumpAction = new InputAction("Jump", InputActionType.Button);
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Gamepad>/buttonSouth");

        crouchAction = new InputAction("Crouch", InputActionType.Button);
        crouchAction.AddBinding("<Keyboard>/c");
        crouchAction.AddBinding("<Gamepad>/buttonEast");

        sprintAction = new InputAction("Sprint", InputActionType.Button);
        sprintAction.AddBinding("<Keyboard>/leftShift");
        sprintAction.AddBinding("<Gamepad>/leftStickPress");

        attackAction = new InputAction("Attack", InputActionType.Button);
        attackAction.AddBinding("<Mouse>/leftButton");
        attackAction.AddBinding("<Gamepad>/rightTrigger");
    }

    void HandleMouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * mouseSensitivity * 0.05f;
        float mouseX = look.x;
        transform.Rotate(0f, mouseX, 0f);

        float mouseY = look.y;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (cameraHolder != null)
            cameraHolder.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        if (crouchAction.WasPressedThisFrame())
        {
            isCrouching = !isCrouching;
            cc.height = isCrouching ? crouchHeight : standHeight;

            if (swatModel != null)
            {
                float currentY = swatModel.localPosition.y;
                swatModel.localPosition = isCrouching
                    ? new Vector3(0, currentY + 0.4f, 0)
                    : new Vector3(0, currentY - 0.4f, 0);
            }
        }

        isRunning = sprintAction.IsPressed()
                    && currentStamina > 0
                    && !isCrouching;

        float speed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        cc.Move(move * speed * Time.deltaTime);

        bool wasGrounded = isGrounded;
        isGrounded = CheckGrounded();

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpAction.WasPressedThisFrame() && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isGrounded = false;
            effects?.PlayJump();

            if (animator != null)
                animator.CrossFadeInFixedTime("Jump", 0.02f, 0, 0f);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        isGrounded = CheckGrounded();

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (!wasGrounded && isGrounded)
            effects?.PlayLand();

        if (attackAction.WasPressedThisFrame())
            effects?.FireWeapon();
    }

    bool CheckGrounded()
    {
        if (cc.isGrounded)
            return true;

        float rayDistance = (cc.height * 0.5f) - cc.radius + 0.25f;
        return Physics.SphereCast(transform.position, cc.radius * 0.9f, Vector3.down,
                                  out _, rayDistance);
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
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveX = moveInput.x;
        float moveZ = moveInput.y;
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

    void UpdateAnimator()
    {
        if (animator == null)
            return;

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveX = moveInput.x;
        float moveZ = moveInput.y;
        float speed = Mathf.Abs(moveX) + Mathf.Abs(moveZ);

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VelocityY", velocity.y);
    }

    public void PlayFootstepFromAnimation()
    {
        if (currentState == PlayerState.Walk || currentState == PlayerState.Run || currentState == PlayerState.Crouch)
            effects?.PlayFootstep();
    }
}
