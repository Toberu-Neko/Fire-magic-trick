using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawn_Pipe : MonoBehaviour
{
    public enum spawnState
    {
        Stop,
        Active,
        Max,
    }
    public enum spawnMode
    {
        Keep,
        Once,
    }
    [MMReadOnly] public spawnState state;
    [Header("Setting")]
    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float force;
    [Header("Mode")]
    public spawnMode mode;
    [Header("KeepSpawn")]
    private bool isTimer;
    private float timer;
    [SerializeField] private float SpawnCD;
    [Header("OnceSpawn")]
    [SerializeField] private int number;
    [SerializeField] private bool isSpawn;
    [SerializeField] private bool isFightOver;
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player keep;
    [SerializeField] private MMF_Player once;

    private List<GameObject> enemys = new List<GameObject>();

    private void Update()
    {
        spawnTimer();
    }
    public void ToSpawn()
    {
        state = spawnState.Active;
        keep.PlayFeedbacks();

        switch (mode)
        {
            case spawnMode.Keep:
                spawn();
                toTimer();
                break;

            case spawnMode.Once:
                spawn();
                toTimer();
                break;
        }
    }
    public void StopSpawn()
    {
        state = spawnState.Stop;
        keep.StopFeedbacks();
        setIsTimer(false);
    }
    private void spawnTimer()
    {
        if(isTimer)
        {
            timer += Time.deltaTime;
        }

        if(timer > SpawnCD)
        {
            timer = 0;
            timerEnd();
        }
    }
    private void toTimer()
    {
        timer = 0;
        setIsTimer(true);
    }
    private void timerEnd()
    {
        spawn();
    }
    private void spawn()
    {
        GameObject enemy = takeTarget();

        if(enemy!=null)
        {
            once.PlayFeedbacks();
            AgentController agent = enemy.GetComponent<AgentController>();
            enemy.GetComponent<EnemyHealthSystem>().Rebirth(SpawnPoint.position, SpawnPoint.transform.rotation);
            enemy.SetActive(true);
            agent.DisableAgent();
            enemy.GetComponent<Rigidbody>().AddForce(SpawnPoint.forward * force * 1000);

            if(mode == spawnMode.Once)
            {
                enemy.GetComponent<EnemyHealthSystem>().OnEnemyDeath += onEnemyDeath;
                
            }
        }else
        {
            //Max or error.
            StopSpawn();
            state = spawnState.Max;
        }
    }
    private async void onEnemyDeath()
    {
        await Task.Delay(2000);
        foreach (GameObject enemy in enemys) // is some enemy active, return.
        {
            if (enemy.gameObject.activeSelf == true) return;
        }
        if (state == spawnState.Max)
        {
            isFightOver = true;
            StopSpawn();
        }
    }
    private GameObject takeTarget()
    {
        //to max
        if (enemys.Count >= number)
        {
            return null;
        }
        //obj pool have enemy
        foreach (GameObject enemy in enemys)
        {
            if(enemy.gameObject.activeSelf==false)
            { 
                return enemy;
            }
        }
        // Instantiate new enemy
        GameObject newEnemy = Instantiate(Enemy, SpawnPoint.position, SpawnPoint.transform.rotation);
        newEnemy.GetComponent<EnemyHealthSystem>().SetIsRebirthHide(true);
        enemys.Add(newEnemy);
        return newEnemy;
    }
    #region set
    private void setIsTimer(bool active)
    {
        isTimer = active;
    }
    #endregion
}
