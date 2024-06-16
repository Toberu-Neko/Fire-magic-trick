using UnityEngine;

public class PlayerFSMBaseState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected Core core;
    protected Movement movement;
    protected CollisionSenses collisionSenses;

    protected bool isAnimationFinished;
    protected bool isAnimationStartMovement;
    protected bool isExitingState;

    protected float StartTime;

    private string animBoolName;
    
    public PlayerFSMBaseState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;

        core = player.Core;
        movement = core.GetCoreComponent<Movement>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();

        StartTime = 0f;
    }
    
    public virtual void Enter()
    {
        DoChecks();
        StartTime = Time.time;
        player.Anim.SetBool(animBoolName, true);

        isAnimationFinished = false;
        isExitingState = false;
        isAnimationStartMovement = false;
    }
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }
    public virtual void LogicUpdate()
    {
        // player.Anim.speed = Stats.AnimationSpeed;
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

    protected float targetRotation;

    protected void Rotate(float rotationSpeed, float rotateSmoothTime)
    {
        /*
        float vertical = player.InputHandler.RawMovementInput.y;
        float horizontal = player.InputHandler.RawMovementInput.x;

        Vector3 inputDr = movement.ParentTransform.forward * vertical + movement.ParentTransform.right * horizontal;

        movement.ParentTransform.forward = Vector3.Lerp(movement.ParentTransform.forward, inputDr.normalized, rotationSpeed * Time.deltaTime);
        */
        targetRotation = Mathf.Atan2(player.InputHandler.RawMovementInput.x, player.InputHandler.RawMovementInput.y) * Mathf.Rad2Deg
            + player.InputHandler.MainCam.transform.eulerAngles.y;

        float rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotation,ref rotationSpeed, rotateSmoothTime);

        movement.Rotate(rotation);
    }
}
