using UnityEngine;

public interface IDamageable
{
    void Damage(float damageAmount, Vector3 damagePosition, bool trueDamage = false);

    GameObject GetGameObject();
}