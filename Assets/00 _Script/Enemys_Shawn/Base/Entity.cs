using UnityEngine;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public EnemyStateMachine StateMachine { get; private set; }
    [SerializeField] private D_Entity EntityData;
    //[SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private Collider col;

    [field: SerializeField] public GameObject DeathParticles { get; private set; }

    //Core and Core Components
    public Core Core { get; private set; }
    protected Movement movement;

    //Animation
    public Animator Anim { get; private set; }

    //public Stats Stats { get; private set; }
    //protected Combat Combat { get; private set; }
    //public bool SkillCollideDamage { get; private set; }
    //public event Action OnDefeated;

    public virtual void Awake()
    {
        StateMachine = new();
        Core = GetComponentInChildren<Core>();

        movement = Core.GetCoreComponent<Movement>();
        //Stats = Core.GetCoreComponent<Stats>();
        //Combat = Core.GetCoreComponent<Combat>();

        Anim = GetComponent<Animator>();
        if (Anim == null)
        {
            Anim = GetComponentInChildren<Animator>();
        }

        //collisionAttackDetails = EntityData.collisionAttackDetails;

        //movement.OrginalGravityScale = EntityData.gravityScale;
    }

    protected virtual void OnEnable()
    {
        //Stats.OnTimeStopStart += HandleOnTimeStop;
        //Stats.OnTimeStopEnd += HandleOnTimeStart;
        //Stats.Health.OnCurrentValueZero += HandleHealthZero;
        //Combat.OnDamaged += HandleOnDamaged;

        //Stats.Health.Init();
        //Stats.Stamina.Init();

    }

    private void HandleOnDamaged()
    {
        if (EntityData.damagedSFX != null)
        {
            //AudioManager.Instance.PlaySoundFX(EntityData.damagedSFX, transform, AudioManager.SoundType.threeD);
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDisable()
    {
        /*
        Stats.OnTimeStopStart -= HandleOnTimeStop;
        Stats.OnTimeStopEnd -= HandleOnTimeStart;
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
        Combat.OnDamaged -= HandleOnDamaged;
        */

        Anim.speed = 1f;
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        //Anim.SetFloat("yVelocity", movement.RB.velocity.y);
        //Anim.speed = Stats.AnimationSpeed;
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        Core.PhysicsUpdate();

        StateMachine.CurrentState.DoChecks();
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void OnDrawGizmos()
    {
    }


    private void HandleOnTimeStop()
    {
        StateMachine.SetCanChangeState(false);
    }
    private void HandleOnTimeStart()
    {
        StateMachine.SetCanChangeState(true);
    }

    private void AnimationActionTrigger()
    {
        StateMachine.CurrentState.AnimationActionTrigger();
    }
    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }
    private void AnimationStartMovement()
    {
        StateMachine.CurrentState.AnimationStartMovementTrigger();
    }
    private void AnimationStopMovement()
    {
        StateMachine.CurrentState.AnimationStopMovementTrigger();
    }
    private void AnimationDangerParticle()
    {
        StateMachine.CurrentState.AnimationDangerParticleTrigger();
    }


    public void SetFacingDirection(int dir)
    {
        if (movement == null)
        {
            Debug.LogError("movement is null");
            return;
        }
        //movement.CheckIfShouldFlip(dir);
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }

    public void DoDamageToDamageList(float damageAmount, float damageStaminaAmount, Vector2 knockBackAngle, float knockBackForce, bool blockable = true)
    {
        /*
        if (Combat.DetectedDamageables.Count > 0)
        {
            foreach (IDamageable damageable in Combat.DetectedDamageables.ToList())
            {
                damageable.Damage(damageAmount, GetPosition(), blockable);
            }
        }

        if (Combat.DetectedKnockbackables.Count > 0)
        {
            foreach (IKnockbackable knockbackable in Combat.DetectedKnockbackables.ToList())
            {
                knockbackable.Knockback(knockBackAngle, knockBackForce, movement.ParentTransform.position, blockable);
            }
        }

        if (Combat.DetectedStaminaDamageables.Count > 0)
        {
            foreach (IStaminaDamageable staminaDamageable in Combat.DetectedStaminaDamageables.ToList())
            {
                staminaDamageable.TakeStaminaDamage(damageStaminaAmount, GetPosition(), blockable);
            }
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.collider.CompareTag("Player") && Stats.Health.CurrentValue > 0f)
        {
            if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(collisionAttackDetails.knockbackAngle, collisionAttackDetails.knockbackForce, movement.ParentTransform.position, false);
            }
            if (collision.gameObject.TryGetComponent(out IDamageable damageable) && (EntityData.collideDamage || SkillCollideDamage))
            {
                damageable.Damage(collisionAttackDetails.damageAmount, GetPosition(), false);
            }
        }
        */
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        if (collision.collider.CompareTag("Player") && Stats.Health.CurrentValue > 0f)
        {
            if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(collisionAttackDetails.knockbackAngle, collisionAttackDetails.knockbackForce, movement.ParentTransform.position, false);
            }
            if (collision.gameObject.TryGetComponent(out IDamageable damageable) && (EntityData.collideDamage || SkillCollideDamage))
            {
                damageable.Damage(collisionAttackDetails.damageAmount, GetPosition(), false);
            }
        }
        */
    }

    //public void SetSkillCollideDamage(bool value) => SkillCollideDamage = value;

    public float GetColliderX()
    {
        return 0;
        //return col.size.x + col.offset.x;
    }

    public float GetColliderY()
    {
        return 0;
        //return col.size.y + col.offset.y;
    }

    private void HandleHealthZero()
    {
        //OnDefeated?.Invoke();
    }
}
