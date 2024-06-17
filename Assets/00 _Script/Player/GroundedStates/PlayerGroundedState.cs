using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerFSMBaseState
{
    protected int xInput;
    protected int yInput;
    protected Vector2 rawMovementInput;

    private bool jumpInput;
    private bool dashInput;
    protected bool isGrounded;
    protected bool isOnSlope;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenses.Ground;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        rawMovementInput = player.InputHandler.RawMovementInput;

        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;

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
            else if (dashInput)
            {
                // stateMachine.ChangeState(player.DashState);
            }
        }
    }

}
