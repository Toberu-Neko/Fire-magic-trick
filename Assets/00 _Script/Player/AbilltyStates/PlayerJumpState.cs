using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public int AmountOfJumpsLeft { get; private set; }
    private float jumpVelocity;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        ResetAmountOfJumpsLeft();
        jumpVelocity = playerData.jumpVelocity;
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;

        movement.SetVelocityY(jumpVelocity);

        AmountOfJumpsLeft--;
        player.InputHandler.UseJumpInput();
        player.InAirState.SetIsJumping();

        isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();

        jumpVelocity = playerData.jumpVelocity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement.SetVelocityY(playerData.jumpVelocity);
    }

    public bool CanJump()
    {
        return AmountOfJumpsLeft > 0;
    }

    public void ResetAmountOfJumpsLeft()
    {
        AmountOfJumpsLeft = playerData.amountOfJumps;
    }

    public void DecreaseAmountOfJumpsLeft() => AmountOfJumpsLeft--;

    public void SetJumpVelocity(float jumpVelocity) => this.jumpVelocity = jumpVelocity;

}
