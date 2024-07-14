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

        player.SetColliderAndModel(false);
        player.VFXController.SetSuperDashVFX(true);

        if (collisionSenses.Ground)
        {
            isAbilityDone = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderAndModel(true);
        player.VFXController.SetSuperDashVFX(false);

        foreach (var col in SphereDetection(playerData.longRangeDetectRadius))
        {
            if (col != null)
            {
                col.TryGetComponent(out IKnockbackable knockbackable);
                knockbackable?.Knockback(player.transform.position, playerData.superJumpFireJumpKnockbackForce);
                col.TryGetComponent(out IDamageable damageable);
                damageable?.Damage(playerData.superJumpFireDamage, player.transform.position);
            }
        }
        player.VFXController.ActivateFireLandVFX();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(movement.CurrentVelocityXZMagnitude <= playerData.fireballSpeed || useOverrideSpeed)
        {
            useOverrideSpeed = true;
            MoveRelateWithCam(playerData.fireballSpeed);
        }
        else
        {
            if(movement.CurrentVelocityXZMagnitude < maxSpeed)
            {
                MoveRelateWithCam(playerData.fireballForceValue, 0f, true, false);
                maxSpeed = movement.CurrentVelocityXZMagnitude;
            }
        }

        movement.SetVelocityY(Mathf.Lerp(playerData.fireballMaxYVelocity, playerData.fireballMinYVelocity, Time.time - StartTime / playerData.fireballMaxTime));

        if (collisionSenses.Ground)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);
    }

}
