using UnityEngine;

public class EnemySpawn_CardMachine : MonoBehaviour
{
    [SerializeField] private EnemySpawn_Pipe pipe;
    private void Start()
    {
        ToSpawn();
    }
    public void ToSpawn()
    {
        pipe.ToSpawn();
    }
}
