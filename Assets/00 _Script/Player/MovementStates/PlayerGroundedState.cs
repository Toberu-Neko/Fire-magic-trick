using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerFSMBaseState
{
    protected int xInput;
    protected int yInput;

    private bool jumpInput;
    private bool dashInput;
    private bool isGrounded;
    protected bool isOnSlope;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
    }

}
