using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerGroundedState
{
    public PlayerRunningState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        MoveRelateWithCam(playerData.slowRunSpeed, playerData.fastRunSpeed);

        CheckPlayStepSound(2.5f / movement.CurrentVelocityXZMagnitude);

        if (!isExitingState)
        {
            if (MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (player.InputHandler.SprintInput)
            {
                player.InputHandler.UseSprintInput();
                stateMachine.ChangeState(player.WalkingState);
            }
            else if (aimInput)
            {
                stateMachine.ChangeState(player.AimWalkingState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);
    }
}
