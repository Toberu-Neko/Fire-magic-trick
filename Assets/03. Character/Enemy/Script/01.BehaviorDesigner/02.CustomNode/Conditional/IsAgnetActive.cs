using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class IsAgnetActive : Conditional
{
    [Header("Reverse")]
    [SerializeField] private bool reverse;

    NavMeshAgent navMeshAgent;

    public override void OnStart()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!reverse)
        {
            if (navMeshAgent.enabled)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        else
        {
            if (navMeshAgent.enabled)
            {
                return TaskStatus.Failure;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
    }
}