using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage;

    [Header("KickBack")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private Transform knockBackCoordinate;
    [SerializeField] private bool isVertical;

    private void Start()
    {
        if(knockBackCoordinate == null)
        {
            knockBackCoordinate = this.transform.parent.parent;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(damage, transform.position);

            collider.TryGetComponent(out IKnockbackable knockBackable);
            knockBackable?.Knockback(knockBackCoordinate.position, knockbackForce);
        }
    }
}
