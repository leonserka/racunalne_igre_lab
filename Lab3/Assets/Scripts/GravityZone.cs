using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public float gravityScale = 0.2f;

    private bool playerInside = false;
    private Vector3 normalGravity;

    void Start()
    {
        normalGravity = Physics.gravity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Physics.gravity = normalGravity * gravityScale;
        playerInside = true;

        GameManager.Instance?.ShowStatus("LOW GRAVITY ZONE!", 1.5f);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!playerInside) return;

        Physics.gravity = normalGravity;
        playerInside = false;

        GameManager.Instance?.ShowStatus("Normal gravity.", 1f);
    }

    void OnDestroy()
    {
        if (playerInside)
            Physics.gravity = normalGravity;
    }
}
