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

        staminaBar.value = playerController.currentStamina;

        staminaText.text = "Stamina: " + 
            Mathf.Round(playerController.currentStamina) + " / " + 
            playerController.maxStamina;

        stateText.text = "State: " + playerController.currentState.ToString();
    }
}