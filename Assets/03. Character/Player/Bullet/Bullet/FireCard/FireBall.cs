using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("FireBall")]
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(damage, transform.position);
        }
    }
}
