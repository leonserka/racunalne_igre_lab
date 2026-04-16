using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 1.05f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isGrounded;

    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        var map = new InputActionMap("Player");

        moveAction = map.AddAction("Move", binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up",    "<Keyboard>/w")
            .With("Down",  "<Keyboard>/s")
            .With("Left",  "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        jumpAction = map.AddAction("Jump", binding: "<Keyboard>/space");
        jumpAction.AddBinding("<Gamepad>/buttonSouth");

        jumpAction.performed += _ => jumpPressed = true;

        map.Enable();
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance + 0.1f,
            groundLayer
        );
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpPressed = false;
    }

    public void SetSpeed(float newSpeed) => moveSpeed = newSpeed;
    public float GetSpeed() => moveSpeed;
}
