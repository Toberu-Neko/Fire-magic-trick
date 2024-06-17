using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public int AmountOfJumpsLeft { get; private set; }
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        ResetAmountOfJumpsLeft();
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;

        movement.SetVelocityY(playerData.jumpVelocity);
        AmountOfJumpsLeft--;
        player.InputHandler.UseJumpInput();
        player.InAirState.SetIsJumping();

        isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();

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

}
