using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterSuperDashJump : PlayerAbilityState
{
    private int currentFrame;

    public PlayerAfterSuperDashJump(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        movement.SetCanSetVelocity(true);
        movement.SetVelocityY(playerData.superDashJumpVelocity);
        player.InAirState.SetAirControlSpeed(playerData.walkSpeed);
        player.InAirState.SetIsJumping();
        player.JumpState.DecreaseAmountOfJumpsLeft();

        foreach(var col in Physics.OverlapBox(player.transform.position, playerData.superDashFootDetectBox, player.transform.rotation))
        {
            if (!col.CompareTag("Player"))
            {
                col.TryGetComponent(out IKnockbackable knockbackable);
                knockbackable?.Knockback(Vector3.down,  playerData.superDashJumpKnockbackSpeed, player.transform.position);
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Wait for animition to start
        if (currentFrame >= 2)
        {
            isAbilityDone = true;
        }
        else
        {
            currentFrame++;
        }
    }
}
