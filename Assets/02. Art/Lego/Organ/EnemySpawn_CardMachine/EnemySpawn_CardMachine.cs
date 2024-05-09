using UnityEngine;

public class EnemySpawn_CardMachine : MonoBehaviour
{
    [SerializeField] private EnemySpawn_Pipe pipe;
    public bool NeverLoseAggro;
    public void ToSpawn()
    {
        if(NeverLoseAggro)
        {
            pipe.isNeverLoseAggro = true;
        }
        pipe.ToSpawn();
    }
    public void ToStop()
    {
        pipe.Initialization();
    }
}
