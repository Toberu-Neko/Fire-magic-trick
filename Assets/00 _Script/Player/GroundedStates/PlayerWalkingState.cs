using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerGroundedState
{
    private Vector3 v3Workspace;
    private Vector2 v2Workspace;
    public PlayerWalkingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        
        if(v3Workspace.magnitude > 1f)
        {
            v3Workspace.Normalize();
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        if (v3Workspace.magnitude != 0f)
        {
            Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);

            float speed = playerData.moveSpeed * v3Workspace.magnitude;
            v2Workspace.Set(targetDirection.x, targetDirection.z);

            Move(speed, v2Workspace);
        }


        if (!isExitingState)
        {
            if(rawMovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (player.InputHandler.SprintInput)
            {
                player.InputHandler.UseSprintInput();
                stateMachine.ChangeState(player.RunningState);
            }
        }
    }

}
