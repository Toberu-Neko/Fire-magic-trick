using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    [Header("NavMesh")]
    [SerializeField, Tooltip("請放入此物件")] private NavMeshAgent navMeshAgent;
    [SerializeField, Tooltip("地面偵測距離")] private float distance = 0.1f;

    private bool isAgentDisabled = false;

    // 見面三秒即除錯
    void Start()
    {
        if (navMeshAgent == null)
        {
            Debug.LogError(gameObject.name + "的AgentController沒有放入Agent");
        }
    }

    // 當敵人落地時
    void OnCollisionEnter(Collision collision)
    {
        // 碰到的是障礙物、Agnet關閉中、冷卻完成
        if (isAgentDisabled)
        {
            if(IsOnNavMesh())
            {
                EnableAgent();
            }  
        }
    }

    // 禁用NavMeshAgent
    public void DisableAgent()
    {
        navMeshAgent.enabled = false;
        isAgentDisabled = true;
    }

    // 啟用NavMeshAgent
    void EnableAgent()
    {
        navMeshAgent.enabled = true;
        isAgentDisabled = false;
    }

    bool IsOnNavMesh() // 偵測是否落在NavMesh上
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, distance, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }
}
