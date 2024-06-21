using UnityEngine;

public class EnemyFSMBaseState
{
    //Base Setting for Enemy State Machine.
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected EnemyData enemyData;
    protected Core core;

    //Animation state
    protected bool isAnimationFinished;
    protected bool isAnimationStartMovement;
    protected bool isExitingState;

    //State pass in and out.
    protected float StartTime;
    protected float ExitTime;

    //animation...if we have.
    private string animBoolName;

    public EnemyFSMBaseState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.enemyData = enemyData;
        this.animBoolName = animBoolName;

        core = enemy.Core;

        StartTime = 0f;
        ExitTime = 0f;
    }
    public virtual void Enter()
    {
        DoChecks();
        StartTime = Time.time;
    }
    public virtual void Exit()
    {
        ExitTime = Time.time;
    }
    public virtual void LogicUpdate()
    {
    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationActionTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void AnimationStartMovementTrigger() { isAnimationStartMovement = true; }

    public virtual void AnimationStopMovementTrigger() { isAnimationStartMovement = false; }

    public virtual void AnimationSFXTrigger() { }
}
