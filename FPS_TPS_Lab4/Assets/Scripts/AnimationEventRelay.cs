using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerEffects playerEffects;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerEffects = GetComponentInParent<PlayerEffects>();
    }

    public void Footstep()
    {
        playerController?.PlayFootstepFromAnimation();
    }

    public void JumpSound()
    {
        playerEffects?.PlayJump();
    }

    public void LandDust()
    {
        playerEffects?.PlayLand();
    }
}
