using UnityEngine;

public class EA1_State_Idel : EnemyFSMBaseState
{
    public EA1_State_Idel(Enemy_A1 enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        entity.changeState_Enum(E_State.Idel);
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
