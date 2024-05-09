using UnityEngine;

public class CardMachineManager : MonoBehaviour
{
    private EnemySpawn_CardMachine[] cardMachines;

    private void Awake()
    {
        cardMachines = GetComponentsInChildren<EnemySpawn_CardMachine>();
    }
    public void ToSpawn()
    {
        for(int i=0;i<cardMachines.Length;i++)
        {
            cardMachines[i].ToSpawn();
        }
    }
    public void  ToClose()
    {
        for (int i = 0; i < cardMachines.Length; i++)
        {
            cardMachines[i].ToStop();
        }
    }
}
