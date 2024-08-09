using UnityEngine;

public class WaterPool : MonoBehaviour
{
    [SerializeField] private float damageAmount = 15f;
    [SerializeField] private float knockbackForce = 8f;
    private void Start()
    {
        Destroy(gameObject,2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.TryGetComponent(out IDamageable damageable);
            damageable.Damage(damageAmount, transform.position);
            other.TryGetComponent(out IKnockbackable knockbackable);
            knockbackable.Knockback(transform.position, knockbackForce);
        }    
    }
}
