using UnityEngine;

public class State_Patrol : EnemyFSMBaseState
{
    //reference
    private ED_EnemyPatrol data;
    private Transform enemy;

    //value
    private float speed;
    private bool isTurning;
    private float targetAngle;
    private float turnAngle = 90f;

    public State_Patrol(Entity entity, EnemyStateMachine stateMachine, ED_EnemyPatrol enemyData, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        this.data = enemyData;
        enemy = entity.transform;
    }
    public override void Enter()
    {
        base.Enter();
        speed = data.PatrolSpeed;
        Movement.SetVelocity(speed, enemy.forward);
        isTurning = false;
        targetAngle = 0f;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Navigation.isObstacleForward || isTurning)
        {
            isTurning = true;

            // 計算這一禎的旋轉角度, 1. 目前的Y角度, 2 . 目標角度,
            // 3. 旋轉速度 ( 10 禎 * 1禎的時間, 也就是 1/6 秒, 那這禎的旋轉角度就會是 「 (目標角度 - 目前角度) * 0.16」, 並每禎更新目前角度, 達到滑順的效果.)
            float targetDegree = Mathf.Lerp(enemy.eulerAngles.y, targetAngle, 10f * Time.deltaTime);
            Movement.Rotate(targetDegree);

            if (turnAngle - targetDegree < 1f)
            {
                isTurning = false;
                Movement.Rotate(targetAngle);
            }

            Movement.SetVelocityZero();
        }
        else
        {
            // 計算目標角度, 避免每禎都重新計算
            targetAngle = enemy.eulerAngles.y + turnAngle;
            Movement.SetVelocity(speed, enemy.forward);
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
