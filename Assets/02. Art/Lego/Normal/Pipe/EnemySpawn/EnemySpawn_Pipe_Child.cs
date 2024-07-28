using UnityEngine;

public class EnemySpawn_Pipe_Child : MonoBehaviour
{
    //Script
    [SerializeField] private Barrier barrier;
    [SerializeField] private EnemySpawn_GlassBox glassBox;

    private bool isTrigger;
    private bool isFightOver;

    private void Start()
    {
        GameManager.Instance.OnPlayerReborn += OnPlayerDeath;
        glassBox.OnFightOver += FigthOver;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerReborn -= OnPlayerDeath;
        glassBox.OnFightOver -= FigthOver;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!isTrigger && !isFightOver)
            {
                isTrigger = true;
                startFight();
            }
        }
    }
    private void startFight()
    {
        barrier.Open();
        glassBox.StartFight();
    }
    private void FigthOver()
    {
        isFightOver = true;
        GameManager.Instance.OnPlayerReborn -= OnPlayerDeath;
    }
    private void OnPlayerDeath()
    {
        barrier.Close();
        glassBox.onPlearDeath();
        isTrigger = false;
    }
}
