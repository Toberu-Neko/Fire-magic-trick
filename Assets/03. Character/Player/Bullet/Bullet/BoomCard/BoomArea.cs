using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(Collider))]
public class BoomArea : MonoBehaviour
{
    [Header("Boom Area")]
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private int damage = 0;
    [SerializeField] private float forceToEnemy = 30;
    [SerializeField] private float forceToPlayer = 90;

    private Collider col;
    private float startTime;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        col.enabled = false;
        DelayExplode();
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time >= delay + 0.2f)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

    private async void DelayExplode()
    {
        await Task.Delay((int)(delay * 1000));

        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(damage, transform.position);
        }

        other.TryGetComponent(out IKnockbackable knockbackable);
        knockbackable?.Knockback(transform.position, forceToPlayer);
    }
}
