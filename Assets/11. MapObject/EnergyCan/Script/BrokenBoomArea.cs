using System.Threading.Tasks;
using UnityEngine;

public class BrokenBoomArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(2f, transform.position);

            other.TryGetComponent(out IFlammable flammable);
            flammable?.SetOnFire(3f);

            Debug.LogWarning("Haven't implement variable yet");
        }

        if(other.CompareTag("EnergyCan"))
        {
            if (other.TryGetComponent(out EnergyCan can))
            {
                can.Broke();
            }
        }
    }
}
