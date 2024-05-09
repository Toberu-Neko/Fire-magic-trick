using UnityEngine;

public class EnemySpawn_Manager : MonoBehaviour
{
    private EnemySpawn_Pipe[] pipes;

    private void Awake()
    {
        pipes = GetComponentsInChildren<EnemySpawn_Pipe>();
    }
    public void ToSpawn()
    {
        for(int i=0; i<pipes.Length; i++)
        {
            pipes[i].ToSpawn();
        }
    }
    public void StopSpawn()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].StopSpawn();
        }
    }
}
