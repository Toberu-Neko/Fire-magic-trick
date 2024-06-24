using UnityEngine;

public interface IKnockbackable
{
    void Knockback(Vector3 dir, float force, Vector3 damagePosition);

}