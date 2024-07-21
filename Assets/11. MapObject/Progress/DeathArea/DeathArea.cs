using UnityEngine;

public class DeathArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out IDamageable damageable);
        damageable?.Damage(9999, transform.position, true);
    }
}
