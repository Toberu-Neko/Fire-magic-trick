using UnityEngine;

public class EnemySpawn_GlassBox : MonoBehaviour
{
    //public List<EnemySpawn_Pipe> pipes = new();
    public EnemySpawn_Pipe[] pipes;

    [Header("Enemy")]
    [SerializeField] GameObject Enemy_A;
    [SerializeField] GameObject Enemy_B;
    [SerializeField] GameObject Enemy_C;

    private void Awake()
    {
        pipes = GetComponentsInChildren<EnemySpawn_Pipe>();

        /*
        EnemySpawn_Pipe[] _pipes = GetComponentsInChildren<EnemySpawn_Pipe>();

        foreach (EnemySpawn_Pipe _pipe in _pipes)
        {
            pipes.Add(_pipe);
        }
        */
    }
    public void StartFight()
    {
        FightA();
    }
    public void FightA()
    {
        playPipe(pipes[1], Enemy_A, 2, 0.5f);
        playPipe(pipes[3], Enemy_A, 2, 0.75f);
        playPipe(pipes[5], Enemy_B, 2, 1f);
        playPipe(pipes[7], Enemy_C, 2, 2f);
    }
    public void FightB()
    {
        playPipe(pipes[1], Enemy_A, 2, 0.5f);
        playPipe(pipes[3], Enemy_A, 2, 0.75f);
        playPipe(pipes[5], Enemy_B, 2, 1f);
        playPipe(pipes[7], Enemy_C, 2, 2f);
    }
    public void FightC()
    {

    }
    private void playPipe(EnemySpawn_Pipe pipe,GameObject Enemy,int number,float CD)
    {
        pipe.resetPipe(Enemy,number, CD);
        pipe.ToSpawn();
    }
    /*
    private void reform(int count)
    {
        List<EnemySpawn_Pipe> list = pipes;

        for (int i = 0; i < count; i++)
        {
            int newCount = Random.Range(0, pipes.Count);
            list[newCount].Initialization();
            list[newCount].resetPipe(Enemy, number, CD);
        }
    }
    */
}
