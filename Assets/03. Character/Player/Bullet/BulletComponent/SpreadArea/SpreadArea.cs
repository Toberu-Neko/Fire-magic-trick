using UnityEngine;

public class SpreadArea : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.LogError("Implment variable");
            other.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(5, transform.position);
        }
    }
}
