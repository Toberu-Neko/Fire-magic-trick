using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterSuperDashJump : PlayerAbilityState
{
    public PlayerAfterSuperDashJump(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        movement.SetVelocityY(playerData.superDashJumpVelocity);
        player.InAirState.SetAirControlSpeed(playerData.walkSpeed);
        player.InAirState.SetIsJumping();

        foreach( var col in Physics.OverlapBox(player.transform.position, playerData.superDashFootDetectBox, player.transform.rotation))
        {
            col.TryGetComponent(out IKnockbackable knockbackable);
            knockbackable?.Knockback(Vector3.down,  playerData.superDashJumpKnockbackSpeed, player.transform.position);
        }

        isAbilityDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + playerData.superDashJumpTime)
        {
            isAbilityDone = true;
        }
    }
}
