using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TargetWithinAngle : Conditional
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;

    [Header("Angle")]
    [SerializeField] private float max;

    private float distanceToTarget;

    public override TaskStatus OnUpdate()
    {
        if (isWithinAngle())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    private bool isWithinAngle()
    {
        // 獲取目標位置，並將其投影到水平平面上
        Vector3 targetPosition = GameManager.Instance.Player.position;
        Vector3 targetPositionWithoutY = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // 計算到目標的方向向量
        Vector3 toTarget = targetPositionWithoutY - transform.position;
        toTarget.Normalize();

        // 計算敵人正前方的方向向量
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // 將 y 軸設置為 0，使其在水平平面上

        // 使用向量夾角計算
        float angle = Vector3.Angle(forwardDirection, toTarget);

        if (angle <= max/2  )
        {
            return true;
        }
        return false;
    }
}