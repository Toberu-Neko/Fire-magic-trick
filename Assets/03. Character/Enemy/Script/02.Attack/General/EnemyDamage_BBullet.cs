using UnityEngine;

public class EnemyDamage_BBullet : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage;

    [Header("KickBack")]
    [SerializeField] private float force;
    [SerializeField] private Transform knockBackCoordinate;

    bool trigger;
    Rigidbody rb;

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
            if(!trigger)
            {
                trigger = true;
                collider.TryGetComponent(out IDamageable damageable);
                damageable?.Damage(damage, knockBackCoordinate.position);
                collider.TryGetComponent(out IKnockbackable knockbackable);
                knockbackable?.Knockback(knockBackCoordinate.position, force);
            }
        }
    }
    public void DestroyObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
