using UnityEngine;

public class ToDamagePlayer : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if(health !=null)
            {
                health.ToDamagePlayer(damage);
            }
        }
    }
}
