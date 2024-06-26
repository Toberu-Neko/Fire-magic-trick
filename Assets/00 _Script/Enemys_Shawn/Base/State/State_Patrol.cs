using UnityEngine;

public class State_Patrol : EnemyFSMBaseState
{
    public State_Patrol(Entity entity, EnemyStateMachine stateMachine, ED_EnemyPatrol enemyData, string animBoolName) : base(entity, stateMachine, animBoolName)
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
        Movement.SetVelocity(10,Vector3.forward);
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
}
