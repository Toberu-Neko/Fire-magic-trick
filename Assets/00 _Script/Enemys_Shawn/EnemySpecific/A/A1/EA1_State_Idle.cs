using UnityEngine;

public class EA1_State_Idle : State_Idle
{
    private Enemy_A1 enemy;
    public EA1_State_Idle(Enemy_A1 enemy, EnemyStateMachine stateMachine, ED_EnemyIdleState enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
        this.enemy = enemy;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.S_Patrol);
        }
    }
}
