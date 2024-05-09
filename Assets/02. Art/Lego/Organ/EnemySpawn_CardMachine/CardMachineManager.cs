using UnityEngine;

public class CardMachineManager : MonoBehaviour
{
    private EnemySpawn_CardMachine[] cardMachines;

    private void Awake()
    {
        cardMachines = GetComponentsInChildren<EnemySpawn_CardMachine>();
    }
}
