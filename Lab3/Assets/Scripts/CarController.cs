using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float motorForce = 1500f;
    public float maxSpeed = 18f;
    public float turnSpeed = 60f;

    [Header("Wheels (visual only)")]
    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelRL;
    public Transform wheelRR;

    public bool isDriving = false;

    private Rigidbody rb;
    private float throttleInput;
    private float steerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 800f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 10f;
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (!isDriving) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        throttleInput = 0f;
        if (kb.wKey.isPressed) throttleInput =  1f;
        if (kb.sKey.isPressed) throttleInput = -1f;

        steerInput = 0f;
        if (kb.dKey.isPressed) steerInput =  1f;
        if (kb.aKey.isPressed) steerInput = -1f;

        SpinWheels();
    }

    void FixedUpdate()
    {
        if (!isDriving)
        {
            throttleInput = 0f;
            steerInput = 0f;
            return;
        }

        float targetSpeed = throttleInput * maxSpeed;
        Vector3 targetVel = transform.forward * targetSpeed;
        targetVel.y = rb.linearVelocity.y; 

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVel, 6f * Time.fixedDeltaTime);

        float currentSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        if (currentSpeed > 0.3f)
        {
            float turn = steerInput * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(throttleInput >= 0 ? 1 : -1);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
        }
    }

    void SpinWheels()
    {
        float speed = rb.linearVelocity.magnitude;
        float dir = throttleInput >= 0f ? 1f : -1f;
        float spin = speed * 300f * Time.deltaTime * dir;

        if (wheelFL) wheelFL.Rotate(spin, 0f, 0f, Space.Self);
        if (wheelFR) wheelFR.Rotate(spin, 0f, 0f, Space.Self);
        if (wheelRL) wheelRL.Rotate(spin, 0f, 0f, Space.Self);
        if (wheelRR) wheelRR.Rotate(spin, 0f, 0f, Space.Self);
    }
}
