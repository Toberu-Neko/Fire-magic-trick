using UnityEngine;

public class CheckPointArea : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator?.SetTrigger("Active");
            other.TryGetComponent(out PlayerVariableInterface playerVariableInterface);
            playerVariableInterface?.SetRespawnPosition(transform.position);
        }
    }
}
