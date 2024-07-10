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

        player.ChangeActiveCam(Player.ActiveCamera.Normal);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        MoveRelateWithCam(playerData.walkSpeed);

        if (!isExitingState)
        {
            if(MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (player.InputHandler.SprintInput)
            {
                player.InputHandler.UseSprintInput();
                stateMachine.ChangeState(player.RunningState);
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
