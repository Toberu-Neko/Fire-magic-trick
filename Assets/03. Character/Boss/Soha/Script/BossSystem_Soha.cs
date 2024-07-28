using UnityEngine;

public class BossSystem_Soha : MonoBehaviour
{
    //Script
    private Boss_System system;

    [Header("Start Boss Fight")]
    [SerializeField] private PumberManager pumberManager;
    [SerializeField] private CardMachineManager spawnManager_card;
    [SerializeField] private EnemySpawn_Manager spawnManager_pipe;
    private void Awake()
    {
        system = GetComponent<Boss_System>();
    }
    private void Start()
    {
        system.onStartFight += startBossFight;
        system.onResetFight += resetBossFight;
        system.onEndFight += endBossFight;
    }

    private void startBossFight()
    {
        pumberManager.CloseAllPumbers();
        pumberManager.StartNewBossFight();
        spawnManager_card.ToSpawn();
        spawnManager_pipe.ToSpawn();
    }
    private void resetBossFight()
    {
        pumberManager.CloseAllPumbers();
        pumberManager.StopAllCoroutines();
        spawnManager_card.ToClose();
        spawnManager_pipe.StopSpawn();
    }
    private void endBossFight()
    {
        system.onStartFight -= startBossFight;
        system.onResetFight -= resetBossFight;
        pumberManager.StopAllCoroutines();
        spawnManager_card.ToClose();
        spawnManager_pipe.StopSpawn();
    }
}
