using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballState : PlayerAbilityState
{
    private float maxSpeed;
    private bool useOverrideSpeed;
    public PlayerFireballState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        maxSpeed = movement.CurrentVelocityXZMagnitude;
        useOverrideSpeed = false;

        player.SetCollider(false);
        player.SetPlayerModel(false);
        player.VFXController.SetSuperDashVFX(true);

        if (collisionSenses.Ground)
        {
            isAbilityDone = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetCollider(true);
        player.SetPlayerModel(true);
        player.VFXController.SetSuperDashVFX(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(movement.CurrentVelocityXZMagnitude <= playerData.fireballSpeed || useOverrideSpeed)
        {
            useOverrideSpeed = true;
            MoveAndRotateWithCam(playerData.fireballSpeed);
        }
        else
        {
            if(movement.CurrentVelocityXZMagnitude < maxSpeed)
            {
                MoveAndRotateWithCam(playerData.fireballForceValue, 0f, true, false);
                maxSpeed = movement.CurrentVelocityXZMagnitude;
            }
        }

        movement.SetVelocityY(Mathf.Lerp(playerData.fireballMaxYVelocity, playerData.fireballMinYVelocity, Time.time - StartTime / playerData.fireballMaxTime));

        if (collisionSenses.Ground)
        {
            isAbilityDone = true;
        }
    }
}
