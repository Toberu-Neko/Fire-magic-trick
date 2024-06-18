using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private Vector2 v2Workspace;
    private bool canUseDash;
    private int currentFrame;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseDashInput();

        v2Workspace = new();

        movement.SetGravityZero();
        movement.SetVelocityZero();

        canUseDash = false;
        currentFrame = 0;

        player.ChangeActiveCam(Player.ActiveCamera.Dash);
    }

    public override void Exit()
    {
        base.Exit();

        movement.SetGravityOrginal();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(currentFrame <= 3)
        {
            Rotate(playerData.rotationSpeed * 20f, 0.01f);
        }

        currentFrame += 1;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        v2Workspace.Set(movement.ParentTransform.forward.x, movement.ParentTransform.forward.z);
        movement.SetVelocity(playerData.dashSpeed, v2Workspace);

        if (Time.time > StartTime + playerData.dashTime)
        {
            if (collisionSenses.Ground)
            {
                stateMachine.ChangeState(player.RunningState);
            }
            else
            {
                isAbilityDone = true;
            }
        }
    }

    public bool CanDash() => canUseDash && (Time.time >= ExitTime + playerData.dashCooldown || ExitTime == 0f);

    public void ResetCanDash() => canUseDash = true;
}
