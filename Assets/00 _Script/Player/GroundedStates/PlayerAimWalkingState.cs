using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWalkingState : PlayerGroundedState
{
    public PlayerAimWalkingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.ChangeActiveCam(Player.ActiveCamera.Aim);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime, false);

        MoveWithFacingDir(playerData.walkSpeed);

        if (!isExitingState)
        {
            if (!aimInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.AimIdleState);
            }
        }
    }
}
