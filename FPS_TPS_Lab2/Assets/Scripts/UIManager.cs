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

        // Ažuriraj stamina bar
        staminaBar.value = playerController.currentStamina;

        // Ažuriraj stamina text
        staminaText.text = "Stamina: " + 
            Mathf.Round(playerController.currentStamina) + " / " + 
            playerController.maxStamina;

        // Ažuriraj state text
        stateText.text = "State: " + playerController.currentState.ToString();
    }
}