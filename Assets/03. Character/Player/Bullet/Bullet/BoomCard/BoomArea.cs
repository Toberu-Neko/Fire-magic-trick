using UnityEngine;
using System.Threading.Tasks;

public class BoomArea : MonoBehaviour
{
    [Header("Boom Area")]
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private int damage = 0;
    [SerializeField] private float forceToEnemy = 30;
    [SerializeField] private float forceToPlayer = 90;

    private KickBackEnemy kickBackEnemy;
    private Collider coli;

    private void Start()
    {
        kickBackEnemy = GetComponent<KickBackEnemy>();
        coli = GetComponent<Collider>();

        delayBoom();
        Destroy(this.gameObject, delay+0.2f);
    }
    private async void delayBoom()
    {
        await Task.Delay((int)(delay * 1000));
        if(coli != null)
        {
            coli.enabled = true;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(damage, transform.position);

            if(other.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage, PlayerDamage.DamageType.NormalShoot);
                kickBackEnemy.kickBackEnemy(other, forceToEnemy);
            }
        }

        other.TryGetComponent(out IKnockbackable knockbackable);
        knockbackable?.Knockback(transform.position, forceToPlayer);
    }
}
