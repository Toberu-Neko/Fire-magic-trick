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
    public GameObject Enemy;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float force;
    [Header("Mode")]
    public spawnMode mode;
    public bool isNeverLoseAggro;
    [Header("KeepSpawn")]
    [SerializeField] private bool keepSpawnCheck;
    private bool isTimer;
    private float timer;
    public int number;
    [SerializeField] private float SpawnCD;
    public bool isFightOver;
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player keep;
    [SerializeField] private MMF_Player once;

    private GameObject player;
    public MyDelegates.OnHandler onPipeFightover;
    private List<GameObject> enemys = new List<GameObject>();

    private void Start()
    {
        player = GameManager.Instance.Player.gameObject;
    }
    private void Update()
    {
        spawnTimer();
    }
    public void Initialization()
    {
        StopSpawn();
        isFightOver = false;

        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
        enemys.Clear();
    }
    public void ChangeMode(spawnMode mode)
    {
        this.mode = mode;
    }
    public void resetPipe(GameObject Enemy, int number, float CD)
    {
        Initialization();
        this.Enemy = Enemy;
        this.number = number;
        this.SpawnCD = CD;
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
    public void ClearNavigation()
    {
        foreach(GameObject enemy in enemys)
        {
            enemy.GetComponent<AgentController>().DisableAgent();
        }
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
            enemy.GetComponent<EnemyAggroSystem>().SetAggroTarget(player);
            enemy.SetActive(true);
            if (isNeverLoseAggro) enemy.GetComponent<EnemyAggroSystem>().SettingMaxAggro(9999);
            if(agent != null) agent.DisableAgent();
            enemy.GetComponent<Rigidbody>().AddForce(SpawnPoint.forward * force * 1000);

            if(mode == spawnMode.Once)
            {
                enemy.GetComponent<EnemyHealthSystem>().OnEnemyDeath += onEnemyDeath;
            }
        }else
        {
            //Max or error.
            if (keepSpawnCheck) return;
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
        fightOver();
    }
    private void fightOver()
    {
        isFightOver = true;
        onPipeFightover?.Invoke();
        StopSpawn();
    }
    private GameObject takeTarget()
    {
        //to max
        if (enemys.Count >= number)
        {
            if(!keepSpawnCheck)
            {
                return null;
            }
        }
        //obj pool have enemy
        foreach (GameObject enemy in enemys)
        {
            if(enemy.gameObject.activeSelf==false)
            { 
                return enemy;
            }
        }
        if (enemys.Count >= number)
        {
            return null;
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
