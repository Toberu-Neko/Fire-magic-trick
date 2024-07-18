using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadingState : PlayerAbilityState
{
    private readonly float yVelocity = 0.4f;

    public PlayerLoadingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stats.SetInvincible(true);
        player.UseCameraRotate = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement.SetVelocityY(yVelocity);
    }

    public override void Exit()
    {
        base.Exit();

        stats.SetInvincible(false);
        player.UseCameraRotate = true;
    }

    public void SetIsAbilityDone()
    {
        isAbilityDone = true;
    }
}
