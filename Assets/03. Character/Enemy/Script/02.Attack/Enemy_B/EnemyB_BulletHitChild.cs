using UnityEngine;

public class EnemyB_BulletHitChild : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.LogWarning("Haven't implement burn duration variable.");
            other.TryGetComponent(out IFlammable flammable);
            flammable.SetOnFire(3f);

        }
    }
}
