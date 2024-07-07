using UnityEngine;

public class EnemyHaveEnergyCan : MonoBehaviour
{
    [Header("Can")]
    [SerializeField] private EnergyCan energyCanSystem;

    private EnemyHealthSystem healthSystem;
    private void Awake()
    {
        healthSystem = GetComponent<EnemyHealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnEnemyDeath += EnergyCanBroke;
    }

    private void OnDisable()
    {
        healthSystem.OnEnemyDeath -= EnergyCanBroke;
    }

    private void EnergyCanBroke()
    {
        energyCanSystem.Broke();
    }
}
