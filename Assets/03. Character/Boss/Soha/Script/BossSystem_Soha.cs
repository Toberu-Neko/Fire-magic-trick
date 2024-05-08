using UnityEngine;

public class BossSystem_Soha : MonoBehaviour
{
    //Script
    private Soha soha;
    private Boss_System system;

    [Header("Start Boss Fight")]
    [SerializeField] private PumberManager pumberManager;
    private void Awake()
    {
        soha = GetComponent<Soha>();
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
        pumberManager.StartNewBossFight();
    }
    private void resetBossFight()
    {

    }
    private void endBossFight()
    {
        system.onStartFight -= startBossFight;
        system.onResetFight -= resetBossFight;
    }
}
