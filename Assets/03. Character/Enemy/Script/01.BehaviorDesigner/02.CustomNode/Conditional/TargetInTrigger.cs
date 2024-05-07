using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckTargetInTrigger : Conditional
{
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private Collider triggerCollider;

    public override TaskStatus OnUpdate()
    {
        // 檢查目標物體是否存在並且觸發區域是否存在
        if (targetObject.Value != null && triggerCollider != null)
        {
            // 檢查目標物體是否在觸發區域中
            if (triggerCollider.bounds.Contains(targetObject.Value.transform.position))
            {
                // 如果在觸發區域中，返回成功
                return TaskStatus.Success;
            }
        }

        // 如果目標不存在或者不在觸發區域中，返回失敗
        return TaskStatus.Failure;
    }
}