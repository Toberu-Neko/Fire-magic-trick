using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    private Bullet bullet;
    [SerializeField] private int damage;

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
    }

    private void OnEnable()
    {
        bullet.OnHit += ToDamageEnemy;
        bullet.OnTrigger += ToDamageEnemy;
    }

    private void OnDisable()
    {
        bullet.OnHit -= ToDamageEnemy;
        bullet.OnTrigger -= ToDamageEnemy;
    }

    public enum DamageType
    {
        NormalShoot,
        ChargeShoot,
        FireDash,
        SuperDash,
        Kick
    }

    public void ToDamageEnemy(Collider other)
    {
        other.TryGetComponent(out IDamageable damageable);
        damageable?.Damage(damage, transform.position);

        if (other.gameObject.TryGetComponent<IHealth>(out var _health))
        {
            if(other.gameObject != null)
            {
                _health.TakeDamage(damage, damageType);
            }
        }
    }

    public void ToDamageEnemy(Collision Collision)
    {
        if (Collision.gameObject.TryGetComponent<IHealth>(out var _health))
        {
            if(Collision.gameObject != null)
            {
                _health.TakeDamage(damage, damageType);
            }
        }
    }
}
