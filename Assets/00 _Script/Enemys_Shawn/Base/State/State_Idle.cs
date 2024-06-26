using UnityEngine;

public class State_Idle : EnemyFSMBaseState
{
    protected ED_EnemyIdleState stateData;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;

    protected float idleTime;

    public State_Idle(Entity entity, EnemyStateMachine stateMachine, ED_EnemyIdleState enemyData, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = enemyData;
    }
    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
        isIdleTimeOver = false;
        SetIdleTime();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        if(Time.time >= StartTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
    private void SetIdleTime()
    {
        if(stateData.useRandomIdleTime)
        {
            idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
        }else
        {
            idleTime = stateData.idleTime;
        }
    }
}
