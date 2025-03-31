using UnityEngine;

public class ElementalistIdleToHitAnimation : MonoBehaviour
{
    private Animator animator;
    private int idleCounter = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    // Call this function at the end of the Idle animation via Animation Event
    public void OnIdleAnimationEnd()
    {
        idleCounter++;

        Debug.Log("Idle Animation Finished. Counter: " + idleCounter);

        if (idleCounter == 6)  // Adjust this value if needed
        {
            if (animator != null)
            {
                animator.SetTrigger("hit"); // Ensure transition is properly set up
                Debug.Log("Hit Trigger Set!");
            }
            else
            {
                Debug.LogError("Animator is null!");
            }
        }
        if (idleCounter == 8)  // Adjust this value if needed
        {
            if (animator != null)
            {
                animator.SetTrigger("attack"); // Ensure transition is properly set up
                Debug.Log("Attack Trigger Set!");
            }
    }
    }
}
