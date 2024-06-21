using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerFSMBaseState
{
    private bool jumpInput;
    private bool dashInput;
    protected bool aimInput;

    protected bool isGrounded;
    protected bool isOnSlope;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Anim.ResetTrigger("land");
        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenses.Ground;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
        aimInput = player.InputHandler.AimInput;

        CheckIfShouldShoot();

        if (!isExitingState)
        {
            if (jumpInput)
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (!isGrounded)
            {
                player.InAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.InAirState);
            }
            else if (dashInput && player.DashState.CanDash())
            {
                stateMachine.ChangeState(player.DashState);
            }
        }
    }

}
