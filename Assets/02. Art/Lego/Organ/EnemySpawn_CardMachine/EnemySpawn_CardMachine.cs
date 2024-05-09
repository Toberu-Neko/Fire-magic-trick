using UnityEngine;

public class EnemySpawn_CardMachine : MonoBehaviour
{
    [SerializeField] private EnemySpawn_Pipe pipe;
    public void ToSpawn()
    {
        pipe.ToSpawn();
    }
    public void ToStop()
    {
        pipe.Initialization();
    }
}
