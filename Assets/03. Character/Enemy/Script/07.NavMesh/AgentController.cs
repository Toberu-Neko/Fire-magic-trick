using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    [Header("NavMesh")]
    private NavMeshAgent navMeshAgent;
    [SerializeField, Tooltip("地面偵測距離")] private float distance = 0.1f;

    private bool isAgentDisabled = false;
    private bool firstTime;

    private Core core;
    private Movement movement;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        movement = core.GetCoreComponent<Movement>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        firstTime = true;
    }

    private void Update()
    {
        if (movement.CanSetVelocity)
        {
            if (isAgentDisabled && IsOnNavMesh())
            {
                EnableAgent();
            }
        }
        else if (!movement.CanSetVelocity)
        {
            if(!isAgentDisabled)
            {
                DisableAgent();
            }
        }

    }

    // 當敵人落地時
    void OnCollisionEnter(Collision collision)
    {
        // 碰到的是障礙物、Agnet關閉中、冷卻完成
        if (isAgentDisabled && firstTime)
        {
            firstTime = false;
            if (IsOnNavMesh())
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
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, distance, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }
}
