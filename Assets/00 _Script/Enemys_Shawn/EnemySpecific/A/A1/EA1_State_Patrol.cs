using UnityEngine;

public class EA1_State_Patrol : State_Patrol
{
    public Enemy_A1 enemy;
    public EA1_State_Patrol(Enemy_A1 enemy, EnemyStateMachine stateMachine, ED_EnemyPatrol enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
        this.enemy = enemy;
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
