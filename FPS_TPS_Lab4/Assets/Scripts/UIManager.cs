using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public Slider staminaBar;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI stateText;

    void Update()
    {
        if (playerController == null) return;

        if (staminaBar != null)
        {
            staminaBar.maxValue = playerController.maxStamina;
            staminaBar.value = playerController.currentStamina;
        }

        if (staminaText != null)
        {
            staminaText.text = "Stamina: " +
                Mathf.Round(playerController.currentStamina) + " / " +
                playerController.maxStamina;
        }

        if (stateText != null)
            stateText.text = "State: " + playerController.currentState;
    }
}
