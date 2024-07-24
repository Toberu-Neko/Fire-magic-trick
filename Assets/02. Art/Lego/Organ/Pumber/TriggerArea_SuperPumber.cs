using UnityEngine;

public class TriggerArea_SuperPumber : MonoBehaviour
{
    //Script
    private Pumber pumber;
    //Varable
    private float SuperPumberForce;

    private void Awake()
    {
        pumber = GetComponentInParent<Pumber>();
        SuperPumberForce = pumber.SuperPumberForce;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 damagePos = other.transform.position + Vector3.down;

            other.TryGetComponent(out IKnockbackable knockbackable);
            knockbackable?.Knockback(damagePos, SuperPumberForce);
        }
    }
}
