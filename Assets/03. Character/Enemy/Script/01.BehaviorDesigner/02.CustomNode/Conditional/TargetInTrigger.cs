using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TargetInTrigger : Conditional
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;

    [Header("Reverse")]
    [SerializeField] private bool reverse;

    private Collider triggerCollider;

    public override void OnStart()
    {
        triggerCollider = behaviorObject.Value.Find("MeleeTrigger").GetComponent<Collider>();
    }

    public override TaskStatus OnUpdate()
    {
        // 檢查目標物體是否存在並且觸發區域是否存在
        if (targetObject.Value != null && triggerCollider != null)
        {
            // 檢查目標物體是否在觸發區域中
            if (triggerCollider.bounds.Intersects(new Bounds(targetObject.Value.transform.position, Vector3.zero)))
            {
                if(!reverse)
                {
                    return TaskStatus.Success;
                }
                else
                {
                    return TaskStatus.Failure;
                }
                
            }
        }

        if(!reverse)
        {
           return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}