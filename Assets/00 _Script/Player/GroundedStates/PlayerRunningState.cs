using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerGroundedState
{
    private Vector2 v2Workspace;
    private Vector3 v3Workspace;
    public PlayerRunningState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        v3Workspace = new();
        v2Workspace = new();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        v3Workspace.Set(rawMovementInput.x, 0f, rawMovementInput.y);


        if (v3Workspace.magnitude > 1f)
        {
            v3Workspace.Normalize();
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        if (v3Workspace.magnitude != 0f)
        {
            Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);

            float speed = Mathf.Lerp(playerData.slowRunSpeed, playerData.fastRunSpeed, v3Workspace.magnitude);
            v2Workspace.Set(targetDirection.x, targetDirection.z);

            Move(speed, v2Workspace);
            player.InAirState.SetAirControlSpeed(speed);
        }


        if (!isExitingState)
        {
            if (rawMovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (player.InputHandler.SprintInput)
            {
                player.InputHandler.UseSprintInput();
                stateMachine.ChangeState(player.WalkState);
            }
        }
    }
}
