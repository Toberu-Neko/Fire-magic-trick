using UnityEngine;

public class DeathArea : MonoBehaviour
{
    private EnergySystem energySystem;

    private void Start()
    {
        energySystem = GameManager.singleton.Player.GetComponent<EnergySystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            energySystem.playerDeath();
        }
        if(other.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealthSystem = other.GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem != null )
            {
                enemyHealthSystem.EnemyDeathRightNow();
            }
        }
    }
}
