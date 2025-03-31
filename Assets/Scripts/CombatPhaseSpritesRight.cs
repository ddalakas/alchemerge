using UnityEngine;

public class CombatPhaseAnimationHandlerRight : MonoBehaviour
{
    private Animator animator;
    private Player player; // Reference to the player instance

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = PlayerManager.player2;
        AssignAnimator();
    }

    void AssignAnimator()
    {
        if (player == null || animator == null)
            return;

        string path = "Animations/right/"; // Adjust based on your folder structure
        RuntimeAnimatorController loadedAnimator = null;

        switch (player.baseElement)
        {
            case Player.element.Earth:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Earth/earthRight");
                break;
            case Player.element.Fire:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Fire/fireRight");
                break;
            case Player.element.Water:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Water/waterRight");
                break;
            case Player.element.Wind:
                loadedAnimator = Resources.Load<RuntimeAnimatorController>(path + "Air/windRight");
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