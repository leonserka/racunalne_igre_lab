using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    public Transform cameraHolder;

    [Header("TPS Settings")]
    public float tpsDistance = 4f;
    public float tpsSmoothSpeed = 5f;
    public float shoulderOffset = 0.7f;

    [Header("FPS Settings")]
    public GameObject weapon;

    private bool isFPS = false;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFPS = !isFPS;
        }
    }

    void LateUpdate()
    {
        if (isFPS)
            HandleFPS();
        else
            HandleTPS();
    }

    void HandleFPS()
    {
        transform.position = cameraHolder.position;
        transform.rotation = cameraHolder.rotation;

        if (weapon != null)
            weapon.SetActive(true);
    }

    void HandleTPS()
    {
        Vector3 desiredPosition = cameraHolder.position
            - cameraHolder.forward * tpsDistance
            + cameraHolder.right * shoulderOffset
            + Vector3.up * 0.5f;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            tpsSmoothSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            cameraHolder.rotation,
            tpsSmoothSpeed * Time.deltaTime
        );

        if (weapon != null)
            weapon.SetActive(false);
    }
}