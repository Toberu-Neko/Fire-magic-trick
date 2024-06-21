using UnityEngine;

public class EnemySpawn_Pipe_Child : MonoBehaviour
{
    //Script
    [SerializeField] private Barrier barrier;
    [SerializeField] private EnemySpawn_GlassBox glassBox;

    private ProgressSystem progress;
    private bool isTrigger;
    private bool isFightOver;

    private void Start()
    {
        progress = GameManager.Instance.GetComponent<ProgressSystem>();

        progress.OnPlayerDeath += onPlayerDeath;
        glassBox.OnFightOver += figthOver;
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
    private void figthOver()
    {
        isFightOver = true;
        progress.OnPlayerDeath -= onPlayerDeath;
    }
    private void onPlayerDeath()
    {
        barrier.Close();
        glassBox.onPlearDeath();
        isTrigger = false;
    }
}
