using UnityEngine;

public class EnemyPushPlayer_IgnoreInvicible : MonoBehaviour
{
    [Header("KickBack")]
    [SerializeField] private float force;
    [SerializeField] private Transform knockBackCoordinate;
    [SerializeField] private bool isVertical;

    private IKnockbackable knockbackable;

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
            if(knockbackable != null)
            {
                collider?.TryGetComponent(out knockbackable);
            }

            if (isVertical)
            {
                knockbackable.Knockback(collider.transform.position + Vector3.down, force);
            }
            else
            {
                knockbackable.Knockback(transform.position, force);
            }
        }
    }
}
