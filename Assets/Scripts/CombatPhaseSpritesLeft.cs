using UnityEngine;

public class CombatPhaseAnimationHandlerLeft : MonoBehaviour
{
    private Animator animator;
    private Player player; // Reference to the player instance

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = PlayerManager.player1;
        AssignAnimator();
    }

    void AssignAnimator()
    {
        if (player == null || animator == null)
            return;

        string path = "Animations/left/"; // Adjust based on your folder structure
        RuntimeAnimatorController loadedAnimator = null;

        switch (player.baseElement)
        {
            case Player.element.Earth:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Earth/earthLeft");
                break;
            case Player.element.Fire:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Fire/fireLeft");
                break;
            case Player.element.Water:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Water/waterLeft");
                break;
            case Player.element.Wind:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Air/windLeft");
                break;
        }

        if (loadedAnimator != null)
        {
            animator.runtimeAnimatorController = loadedAnimator;
        }
        else
        {
            Debug.LogError("Failed to load AnimatorController for " + player.baseElement);
        }
    }
}