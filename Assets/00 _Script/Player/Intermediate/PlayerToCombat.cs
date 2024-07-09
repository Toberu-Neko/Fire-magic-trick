using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToCombat : MonoBehaviour, IDamageable, IKnockbackable, IFlammable
{
    [SerializeField] private Core core;
    private Combat combat;

    private void OnEnable()
    {
        combat = core.GetCoreComponent<Combat>();
    }
    public bool CheckIfOnFire()
    {
        return combat.CheckIfOnFire();
    }

    public void Damage(float damageAmount, Vector3 damagePosition)
    {
        combat.Damage(damageAmount, damagePosition);
    }

    public GameObject GetGameObject()
    {
        return combat.GetGameObject();
    }

    public void Knockback(Vector3 dir, float force, Vector3 damagePosition)
    {
        combat.Knockback(dir, force, damagePosition);
    }

    public void Knockback(Vector3 damagePosition, float force, bool knockUp)
    {
        combat.Knockback(damagePosition, force);
    }

    public void SetOnFire(float duration)
    {
        combat.SetOnFire(duration);
    }
}
