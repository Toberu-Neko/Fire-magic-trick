using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn_Pipe : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float force;
    [Header("Mode")]
    [SerializeField] private bool KeepSpawn;
    [SerializeField] private bool OnceSpawn;
    [Header("KeepSpawn")]
    [SerializeField] private bool isTimer;
    [SerializeField] private float timer;
    [SerializeField] private float SpawnCD;
    [SerializeField] public List<GameObject> enemys = new List<GameObject>();

    private void Update()
    {
        spawnTimer();
    }
    public void SpawnStart()
    {
        timer = 0;
        setIsTimer(true);
    }
    public void SpawnEnd()
    {
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
    private void timerEnd()
    {
        spawn();
    }
    private void spawn()
    {
        GameObject enemy = TakeTarget();
        AgentController agent = enemy.GetComponent<AgentController>();
        enemy.GetComponent<EnemyHealthSystem>().Rebirth(SpawnPoint.position, SpawnPoint.transform.rotation);
        enemy.SetActive(true);
        agent.DisableAgent();
        enemy.GetComponent<Rigidbody>().AddForce(SpawnPoint.forward * force*1000);
    }
    private GameObject TakeTarget()
    {
        foreach(GameObject enemy in enemys)
        {
            if(enemy.gameObject.activeSelf==false)
            { 
                return enemy;
            }
        }
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
