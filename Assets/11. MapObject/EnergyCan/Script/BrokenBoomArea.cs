using System.Threading.Tasks;
using UnityEngine;

public class BrokenBoomArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealthSystem = other.GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem !=null) enemyHealthSystem.TakeDamage(2, PlayerDamage.DamageType.SuperDash);
        }
        if(other.CompareTag("EnergyCan"))
        {
            Boom(other);
        }
    }
    private async void Boom(Collider other)
    {
        await Task.Delay(150);
        EnergyCan can = other.GetComponent<EnergyCan>();
        if(can != null)
        {
            can.Broke();
        }
    }
}
