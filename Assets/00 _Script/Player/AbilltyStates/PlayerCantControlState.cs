using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCantControlState : PlayerAbilityState
{
    public PlayerCantControlState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (collisionSenses.Ground)
        {
            movement.SetVelocityZero();
        }
        stats.SetInvincible(true);
        player.UseCameraRotate = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (collisionSenses.Ground)
        {
            movement.SetVelocityZero();
        }
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
