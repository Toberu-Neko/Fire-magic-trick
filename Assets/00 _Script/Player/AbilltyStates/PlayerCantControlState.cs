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
        player.InAirState.SetIsJumpingFalse();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (collisionSenses.Ground)
        {
            player.Anim.SetBool("idle", true);
            player.Anim.SetBool("inAir", false);
            movement.SetVelocityZero();
        }
        else
        {
            player.Anim.SetBool("idle", false);
            player.Anim.SetBool("inAir", true);
            player.Anim.SetTrigger("land");
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
