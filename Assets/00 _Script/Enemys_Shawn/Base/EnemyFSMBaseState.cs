using UnityEngine;

public class EnemyFSMBaseState
{
    //Base Setting for Enemy State.
    protected Entity entity;
    protected EnemyStateMachine stateMachine;
    protected Core core;

    //Animation state
    protected bool isAnimationFinished;
    protected bool isAnimationStartMovement;
    protected bool isExitingState;

    //State pass in and out.
    protected float StartTime;
    protected float ExitTime;

    //animation...if we have.
    protected string animBoolName;
    protected bool saidThings;

    //State
    protected Movement Movement { get; private set; }
    protected CollisionSenses CollisionSenses { get; private set; }

    public EnemyFSMBaseState(Entity entity, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        //core and core component
        core = this.entity.Core;
        Movement = core.GetCoreComponent<Movement>();
        CollisionSenses = core.GetCoreComponent<CollisionSenses>();

        //initialize
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

    public virtual void OnHit() { }

    public virtual void AnimationActionTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void AnimationStartMovementTrigger() { isAnimationStartMovement = true; }

    public virtual void AnimationStopMovementTrigger() { isAnimationStartMovement = false; }

    public virtual void AnimationSFXTrigger() { }
    public virtual void AnimationDangerParticleTrigger() { }

    public virtual void Disable()
    {
    }
    public void SetEndTime(float value)
    {
    }
}
