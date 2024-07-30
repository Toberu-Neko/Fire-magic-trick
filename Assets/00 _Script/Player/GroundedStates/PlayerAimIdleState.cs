using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimIdleState : PlayerGroundedState
{
    public PlayerAimIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.ChangeActiveCam(Player.ActiveCamera.Aim);
    }

    public override void Exit()
    {
        base.Exit();

        player.ChangeActiveCam(Player.ActiveCamera.DeterminBySpeed);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!isExitingState)
        {
            if (!aimInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.AimWalkingState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime, false);
    }
}