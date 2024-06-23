using UnityEngine;

public class E_State_Alert : EnemyFSMBaseState
{
    public E_State_Alert(Enemy_A1 enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        entity.changeState_Enum(E_State.Alert);
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
