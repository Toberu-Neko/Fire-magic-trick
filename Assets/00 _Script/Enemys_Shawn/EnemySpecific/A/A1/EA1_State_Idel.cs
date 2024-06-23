using UnityEngine;

public class EA1_State_Idel : State_Idel
{
    private Enemy_A1 enemy;
    public EA1_State_Idel(Enemy_A1 enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
