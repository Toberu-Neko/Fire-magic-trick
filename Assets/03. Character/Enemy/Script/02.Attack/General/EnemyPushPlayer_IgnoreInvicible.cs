using UnityEngine;

public class EnemyPushPlayer_IgnoreInvicible : MonoBehaviour
{
    [Header("KickBack")]
    [SerializeField] private float force;
    [SerializeField] private Transform knockBackCoordinate;
    [SerializeField] private bool isVertical;

    private void Start()
    {
        if (knockBackCoordinate == null)
        {
            knockBackCoordinate = this.transform.parent.parent;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.TryGetComponent(out IKnockbackable _knockbackable);

            if (isVertical)
            {
                _knockbackable?.Knockback(collider.transform.position + Vector3.down, force);
            }
            else
            {
                _knockbackable?.Knockback(transform.position, force);
            }
        }
    }
}
