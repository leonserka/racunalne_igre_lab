using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;
    public float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        GameManager.Instance?.AddScore(scoreValue);
        GameManager.Instance?.ShowStatus("+10 Score!", 1.5f);
        Destroy(gameObject);
    }
}
