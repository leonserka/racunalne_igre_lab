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

    [Header("Weapon")]
    public GameObject weapon;
    public Transform fpsWeaponHolder;
    public Transform tpsWeaponHolder;
    public Vector3 tpsWeaponLocalPosition = new Vector3(0.05f, 0.02f, 0.18f);
    public Vector3 tpsWeaponLocalRotation = new Vector3(0f, -90f, 0f);
    public Vector3 tpsWeaponLocalScale = new Vector3(0.1f, 0.1f, 0.4f);

    private bool isFPS = false;
    private Camera cam;
    private Transform currentWeaponHolder;
    private Vector3 fpsWeaponLocalPosition;
    private Vector3 fpsWeaponLocalRotation;
    private Vector3 fpsWeaponLocalScale;

    void Start()
    {
        cam = GetComponent<Camera>();
        InitializeWeaponHolders();
        ApplyWeaponView(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFPS = !isFPS;
            ApplyWeaponView();
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
    }

    void InitializeWeaponHolders()
    {
        if (weapon == null)
            return;

        if (fpsWeaponHolder == null)
            fpsWeaponHolder = weapon.transform.parent;

        if (tpsWeaponHolder == null && player != null)
        {
            tpsWeaponHolder = FindChildRecursive(player, "mixamorig:RightHand");

            if (tpsWeaponHolder == null)
                tpsWeaponHolder = FindChildRecursive(player, "RightHand");

            if (tpsWeaponHolder == null)
                tpsWeaponHolder = FindChildRecursive(player, "Swat");
        }

        if (tpsWeaponHolder == null)
            tpsWeaponHolder = player;

        fpsWeaponLocalPosition = weapon.transform.localPosition;
        fpsWeaponLocalRotation = weapon.transform.localEulerAngles;
        fpsWeaponLocalScale = weapon.transform.localScale;
    }

    void ApplyWeaponView(bool force = false)
    {
        if (weapon == null)
            return;

        Transform targetHolder = isFPS ? fpsWeaponHolder : tpsWeaponHolder;
        if (targetHolder == null)
            return;

        if (!force && currentWeaponHolder == targetHolder)
            return;

        weapon.transform.SetParent(targetHolder, false);

        if (isFPS)
        {
            weapon.transform.localPosition = fpsWeaponLocalPosition;
            weapon.transform.localEulerAngles = fpsWeaponLocalRotation;
            weapon.transform.localScale = fpsWeaponLocalScale;
        }
        else
        {
            weapon.transform.localPosition = tpsWeaponLocalPosition;
            weapon.transform.localEulerAngles = tpsWeaponLocalRotation;
            weapon.transform.localScale = tpsWeaponLocalScale;
        }

        weapon.SetActive(true);
        currentWeaponHolder = targetHolder;
    }

    Transform FindChildRecursive(Transform parentTransform, string childName)
    {
        if (parentTransform == null)
            return null;

        if (parentTransform.name == childName)
            return parentTransform;

        foreach (Transform child in parentTransform)
        {
            Transform foundChild = FindChildRecursive(child, childName);
            if (foundChild != null)
                return foundChild;
        }

        return null;
    }
}
