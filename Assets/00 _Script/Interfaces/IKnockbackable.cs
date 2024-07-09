using UnityEngine;

public interface IKnockbackable
{
    void Knockback(Vector3 dir, float force, Vector3 damagePosition);
    void Knockback(Vector3 damagePosition, float force, bool KnockUp = true);

}