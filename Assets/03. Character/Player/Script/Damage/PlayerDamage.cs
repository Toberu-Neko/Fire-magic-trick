using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    private Bullet bullet;
    [SerializeField] private float damage;

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
    }

    public void ToDamageEnemy(Collision Collision)
    {
        Collision.gameObject.TryGetComponent(out IDamageable damageable);
        damageable?.Damage(damage, transform.position);
    }
}
