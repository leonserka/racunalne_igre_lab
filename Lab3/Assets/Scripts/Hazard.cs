using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour
{
    public float speedMultiplier = 0.4f;
    public float duration = 2f;

    private bool active = true;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!active) return;

        var pm = collision.gameObject.GetComponent<PlayerMovement>();
        if (pm == null) return;

        active = false;
        StartCoroutine(SlowPlayer(pm));
    }

    private IEnumerator SlowPlayer(PlayerMovement pm)
    {
        float original = pm.GetSpeed();
        pm.SetSpeed(original * speedMultiplier);
        GameManager.Instance?.ShowStatus("SLOW! -60% speed", duration);

        yield return new WaitForSeconds(duration);

        pm.SetSpeed(original);
        active = true;
    }
}
