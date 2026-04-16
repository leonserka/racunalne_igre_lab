using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI statusText;

    private int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ShowStatus(string message, float duration = 2f)
    {
        if (statusText != null)
            StartCoroutine(StatusRoutine(message, duration));
    }

    private IEnumerator StatusRoutine(string message, float duration)
    {
        statusText.text = message;
        yield return new WaitForSeconds(duration);
        statusText.text = "";
    }
}
