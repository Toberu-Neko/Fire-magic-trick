using UnityEngine;

public class EA1_State_Death : State_Death
{
    public EA1_State_Death(Enemy_A1 enemy, EnemyStateMachine stateMachine, EnemyBaseData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
