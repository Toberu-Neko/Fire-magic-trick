using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class EnemySpawn_Pipe : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float force;
    [SerializeField] public List<GameObject> enemys = new List<GameObject>();

    private void Start()
    {

    }
    private void Update()
    {
        test();
    }
    private void test()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Spawn();
        }
    }
    private void Spawn()
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
}
