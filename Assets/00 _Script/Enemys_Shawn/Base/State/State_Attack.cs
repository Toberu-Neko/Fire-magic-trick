using UnityEngine;

public class State_Attack : EnemyFSMBaseState
{
    public State_Attack(Entity entity, EnemyStateMachine stateMachine, EnemyBaseData enemyData, string animBoolName) : base(entity, stateMachine, animBoolName)
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
    public override void DoChecks()
    {
        base.DoChecks();
    }
}
