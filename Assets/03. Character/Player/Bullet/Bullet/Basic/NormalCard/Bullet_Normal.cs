using UnityEngine;
using UnityEngine.VFX;

public class Bullet_Normal : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;
    [Header("Force")]
    [SerializeField] private float knockbackForce;
    [Header("Feedback")]
    [SerializeField] private ParticleSystem addspeedFeedback;
    [Header("VfxPrefab")]
    [SerializeField] private GameObject cardSlashPrefab;
    [SerializeField] private GameObject hitEnemyPrefab;

    private Collider bulletCollider;
    private Rigidbody rb;
    private PlayerDamage _playerDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
        _playerDamage = GetComponent<PlayerDamage>();
    }

    private void OnEnable()
    {
        Initialization();
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
        Vector3 pos = contact.point;

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnergyCan"))
        {
            _playerDamage.ToDamageEnemy(collision);
            TryGetComponent(out IKnockbackable knockbackable);
            knockbackable?.Knockback(transform.position, knockbackForce);

            GameObject enemyhit = Instantiate(hitEnemyPrefab, pos, rot);
            Destroy(enemyhit, 1f);
            UIManager.Instance.HitEnemyEffect();
        }

        NewHit(pos, rot);
        DestroyBullet();
    }

    private void NewHit(Vector3 pos, Quaternion rot)
    {
        var hitVFX = Instantiate(cardSlashPrefab, pos, rot);
        Destroy(hitVFX, 1.5f);
    }

    private void Initialization()
    {
        Destroy(gameObject,lifeTime);
    }

    private void DestroyBullet()
    {
        bulletCollider.enabled = false;
        rb.drag = 100;
        Destroy(gameObject, 0.3f);
    }
}
