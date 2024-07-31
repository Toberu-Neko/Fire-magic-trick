using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour, IHitNotifier, ITriggerNotifier
{
    [Header("Bullet")]
    [SerializeField] protected GameObject hitEnemyPrefab;
    [SerializeField] protected GameObject cardSlashPrefab;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected float speed;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float colliderHitDamage = 3f;

    //Script
    private Rigidbody rb;
    private Collider coli;

    //delegate
    public event MyDelegates.OnHitHandler OnHit;
    public event MyDelegates.OnTriggerHandler OnTrigger;

    //variable
    protected bool useTriggerEnter = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        coli = GetComponent<Collider>();
        //TODO: crosshairUI = GameManager.Instance.UISystem.GetComponent<CrosshairUI>();

        Destroy(gameObject, lifeTime);
    }

    protected virtual void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        int hitLayerMask = 1 << collision.gameObject.layer;
        if((hitLayerMask & whatIsEnemy) != 0)
        {
            return;
        }
        */

        collision.gameObject.TryGetComponent<IDamageable>(out var damageable);
        damageable?.Damage(colliderHitDamage, transform.position);

        OnHitSomething();
        SpawnVFX(cardSlashPrefab, transform.position, Quaternion.identity, 1.5f);
        DestroyBullet();
        UIManager.Instance.HitEnemyEffect();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Bullet hit enemy in trigger, + hit: " + gameObject.name);
            OnTrigger?.Invoke(other);
            OnHitEnemy();
            // crosshairUI.EnemyHitImpluse(this.transform.position);
            SpawnVFX(hitEnemyPrefab, transform.position, Quaternion.identity, 1f);
            if (NeedHitFeedback())
            {
                OnHitSomething();
                SpawnVFX(cardSlashPrefab, transform.position, Quaternion.identity, 1.5f);
            }
            UIManager.Instance.HitEnemyEffect();
        }
    }
    public void OnHitRightNow()
    {
        SpawnVFX(hitEnemyPrefab, transform.position, Quaternion.identity, 1f);
        SpawnVFX(cardSlashPrefab, transform.position, Quaternion.identity, 1.5f);
        DestroyBullet();
    }
    protected virtual bool NeedHitFeedback() { return true; }
    protected virtual void OnHitEnemy() { }
    protected virtual void OnHitSomething() { }
    private void SpawnVFX(GameObject obj, Vector3 pos, Quaternion rot, float time = 1f)
    {
        var hitVFX = Instantiate(obj, pos, rot);
        Destroy(hitVFX, time);
    }
    private void DestroyBullet()
    {
        // Debug.Log("Destroy Bullet");
        coli.enabled = false;
        rb.drag = 100;
        Destroy(gameObject, 0.3f);
    }
    public void SetSpeed(float value)
    {
        speed = value;
    }
}
