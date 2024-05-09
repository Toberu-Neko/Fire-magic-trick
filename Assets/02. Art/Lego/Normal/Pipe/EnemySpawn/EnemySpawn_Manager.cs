using UnityEngine;

public class EnemySpawn_Manager : MonoBehaviour
{
    private EnemySpawn_Pipe[] pipes;
    [SerializeField] private bool isNeverLoseAggro;

    private void Awake()
    {
        pipes = GetComponentsInChildren<EnemySpawn_Pipe>();
    }
    public void ToSpawn()
    {
        for(int i=0; i<pipes.Length; i++)
        {
            if(isNeverLoseAggro)
            {
                pipes[i].isNeverLoseAggro = true;
            }
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
