using System;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable, IFlammable
{
    private float maxKnockbackTime = 0.4f;

    public List<IDamageable> DetectedDamageables { get; private set; } = new();
    public List<IKnockbackable> DetectedKnockbackables { get; private set; } = new();
    public List<IFlammable> DetectedFlammables { get; private set; } = new();

    private Movement movement;
    private CollisionSenses collisionSenses;
    private Stats stats;

    private bool isKnockbackActive;
    private float knockbackStartTime;


    public event Action OnDamaged;
    public event Action<float> OnDamageAmount;
    public event Action OnKnockback;

    protected override void Awake()
    {
        base.Awake();

        stats = core.GetCoreComponent<Stats>();
        movement = core.GetCoreComponent<Movement>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();
    }
    private void OnEnable()
    {
        isKnockbackActive = false;
        knockbackStartTime = 0f;

        DetectedDamageables = new();
        DetectedKnockbackables = new();
        DetectedFlammables = new();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isKnockbackActive)
            CheckKnockback();
    }

    private void CheckKnockback()
    {
        if (isKnockbackActive && ((movement.CurrentVelocity.y <= 0.01f && collisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            movement.SetCanSetVelocity(true);

            // movement.SetKnockbackKinematic();

            isKnockbackActive = false;
        }
    }

    public void Damage(float damageAmount, Vector3 damagePosition)
    {
        if (stats.Health.CurrentValue <= 0 || stats.IsInvincible)
        {
            return;
        }

        stats.Health.Decrease(damageAmount);
        OnDamaged?.Invoke();
        OnDamageAmount?.Invoke(damageAmount);

        /*
        if (stats.Health.CurrentValue <= 0)
        {
            movement.SetVelocity(0f, Vector3.zero);
            movement.SetCanSetVelocity(false);
            movement.SetGravityZero();
        }
        */
    }

    public GameObject GetGameObject()
    {
        return movement.ParentTransform.gameObject;
    }

    #region Knockback
    public void Knockback(Vector3 dir, float force, Vector3 damagePosition)
    {
        dir = dir.normalized;
        HandleKnockback(dir, force);
    }
    public void Knockback(Vector3 damagePosition, float force)
    {
        Vector3 dir = (movement.ParentTransform.position - damagePosition).normalized;
        HandleKnockback(dir, force);
    }

    private void HandleKnockback(Vector3 dir, float strength)
    {
        if (strength == 0f || dir == Vector3.zero || stats.IsInvincible)
        {
            return;
        }

        // Debug.Log("Knockback in Core");

        OnKnockback?.Invoke();

        movement.SetVelocity(strength, dir);
        movement.SetCanSetVelocity(false);
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }
    #endregion

    public void SetOnFire(float duration)
    {
        if (stats.IsInvincible)
        {
            return;
        }

        stats.SetOnFire(duration);
    }

    public bool CheckIfOnFire()
    {
        return stats.IsBurning;
    }


    public void AddToDetected(Collider collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            DetectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Add(knockbackable);
        }

        if (collision.TryGetComponent(out IFlammable flammable))
        {
            DetectedFlammables.Add(flammable);
        }
    }

    public void RemoveFromDetected(Collider collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            DetectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Remove(knockbackable);
        }

        if (collision.TryGetComponent(out IFlammable flammable))
        {
            DetectedFlammables.Remove(flammable);
        }
    }

}
