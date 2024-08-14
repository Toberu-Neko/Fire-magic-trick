using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float force;

    public void Play(Collider other)
    {
        other.TryGetComponent(out IKnockbackable knockbackable);
        knockbackable.Knockback(transform.position, force);
    }
}
