using UnityEngine;

public class CheckPointArea : MonoBehaviour
{
    private Animator animator;
    private float triggerTime;
    private readonly float coolDown = 10f;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        triggerTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time < triggerTime + coolDown && triggerTime != 0f) return;

            triggerTime = Time.time;
            animator?.SetTrigger("Active");
            other.TryGetComponent(out IPlayerHandler player);
            player?.SetRespawnPosition(transform.position);
            DataPersistenceManager.Instance.SaveGame();
        }
    }
}
