using UnityEngine;

public class EA1_State_Alert : EnemyFSMBaseState
{
    public EA1_State_Alert(Enemy_A1 enemy, EnemyStateMachine stateMachine, EnemyBaseData enemyData, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
