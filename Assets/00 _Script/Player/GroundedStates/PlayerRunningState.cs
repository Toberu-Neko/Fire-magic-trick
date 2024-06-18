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

        player.ChangeActiveCam(Player.ActiveCamera.Run);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        MoveAndRotateWithCam(playerData.slowRunSpeed, playerData.fastRunSpeed);

        if (!isExitingState)
        {
            if (MovementInput == Vector2.zero)
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
