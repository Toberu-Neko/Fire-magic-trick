using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderToCombat : MonoBehaviour, IDamageable, IFlammable, IKnockbackable
{
    private Core core;
    private Combat combat;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        combat = core.GetCoreComponent<Combat>();
    }
    public bool CheckIfOnFire()
    {
        return combat.CheckIfOnFire();
    }

    public void Damage(float damageAmount, Vector3 damagePosition, bool trueDamage = false)
    {
        combat.Damage(damageAmount, damagePosition, trueDamage);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Knockback(Vector3 dir, float force, Vector3 damagePosition)
    {
        combat.Knockback(dir, force, damagePosition);
    }

    public void Knockback(Vector3 damagePosition, float force, bool KnockUp = true)
    {
        combat.Knockback(damagePosition, force, KnockUp);
    }

    public void SetOnFire(float duration)
    {
        combat.SetOnFire(duration);
    }
}
