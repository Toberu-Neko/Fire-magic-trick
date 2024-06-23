using UnityEngine;

public class EA1_State_Alert : EnemyFSMBaseState
{
    public EA1_State_Alert(Enemy_A1 enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
