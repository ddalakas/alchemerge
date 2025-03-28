using UnityEngine;

public class ElementalistAnimation : MonoBehaviour
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

        if (idleCounter == 5)  // Adjust this value if needed
        {
            if (animator != null)
            {
                animator.SetTrigger("attack"); // Ensure transition is properly set up
                Debug.Log("Attack Trigger Set!");
            }
            else
            {
                Debug.LogError("Animator is null!");
            }

            
        }
    }
}
