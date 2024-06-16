using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerGroundedState
{
    private Vector3 workspace;
    public PlayerMovementState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        workspace = new();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        workspace.Set(rawMovementInput.x, 0f, rawMovementInput.y);

        if(workspace.magnitude > 1f)
        {
            workspace.Normalize();
        }

        if(workspace.magnitude != 0f)
        {
            movement.SetVelocity(playerData.movementVelocity, workspace);
        }


        if (!isExitingState)
        {
            if(rawMovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
