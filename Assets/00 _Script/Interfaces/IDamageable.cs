using UnityEngine;

public interface IDamageable
{
    void Damage(float damageAmount, Vector3 damagePosition);

    void GotoKinematicState(float time = -1f);

    void GoToStunState();

    GameObject GetGameObject();
}