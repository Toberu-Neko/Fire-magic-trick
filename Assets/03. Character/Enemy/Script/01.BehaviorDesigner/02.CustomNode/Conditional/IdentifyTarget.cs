using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IdentifyTarget : Conditional
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;

    [Header("DetectArea")]
    [SerializeField] private float radius;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    
    [Header("Alert")]
    public float maxAlert = 250;
    public float alert;

    EnemyAggroSystem enemyAggroSystem;

    public override void OnStart()
    {
        
    }

    public override TaskStatus OnUpdate()
    {
        //FieldOfView();
        if (targetObject.Value != null)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    private void FieldOfView()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2 && !Physics.Raycast(transform.position, directionToTarget, radius, obstructionMask))
            {
                targetObject.Value = rangeChecks[0].gameObject;
                
                alert = maxAlert;
            }
            else
            {
                alert--;
            }
        }
        else
        {
            alert--;
        }
    }
}