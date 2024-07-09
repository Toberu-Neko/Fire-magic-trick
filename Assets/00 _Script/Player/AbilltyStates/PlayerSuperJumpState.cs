using UnityEngine;

public class PlayerSuperJumpState : PlayerAbilityState
{
    private float minYVelocity;
    private int currentFrame;
    private bool firstTimeDrop;
    public PlayerSuperJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        currentFrame = 0;
        firstTimeDrop = false;

        player.InputHandler.UseSuperJumpInput();
        player.CardSystem.DecreaseCardEnergy(playerData.superJumpEnergyCost);
        movement.SetVelocityY(playerData.superJumpVelocity);
        minYVelocity = Mathf.Infinity;

        //TODO Different SuperJump Ability
        if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
        {
            // 起跳會聚攏敵人
            foreach(var col in SphereDetection(playerData.longRangeDetectRadius))
            {
                if (col != null)
                {
                    col.TryGetComponent(out IKnockbackable knockbackable);
                    knockbackable?.Knockback((player.transform.position - col.transform.position).normalized, 
                        Vector3.Distance(col.transform.position, player.transform.position) * 3f, player.transform.position);
                }
            }
        }
        else
        {
            // 震退並燃燒敵人

            foreach(var col in SphereDetection(playerData.closeRangeDetectRadius))
            {
                if (col != null)
                {
                    col.TryGetComponent(out IKnockbackable knockbackable);
                    knockbackable?.Knockback(player.transform.position, playerData.superJumpFireKnockbackForce);
                    col.TryGetComponent(out IDamageable damageable);
                    damageable?.Damage(playerData.superJumpFireDamage, player.transform.position);
                }
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentFrame++;

        MoveAndRotateWithCam(playerData.airMoveSpeed);

        if(currentFrame > 5)
        {
            minYVelocity = Mathf.Min(minYVelocity, movement.CurrentVelocity.y);

            if (minYVelocity < -1f)
            {
                if (!firstTimeDrop)
                {
                    firstTimeDrop = true;
                    movement.SetVelocityY(playerData.superJumpFallInitVelocity);
                }

                movement.AddForce(playerData.superJumpFallAddForce, Vector3.down);

                if (collisionSenses.Ground || collisionSenses.Enemy)
                {
                    isAbilityDone = true;
                }
            }
        }
        else
        {
            movement.SetVelocityY(playerData.superJumpVelocity);
        }

    }

    public override void Exit()
    {
        base.Exit();

        foreach (var col in SphereDetection(playerData.longRangeDetectRadius))
        {
            if (col != null)
            {
                col.TryGetComponent(out IKnockbackable knockbackable);
                knockbackable?.Knockback(player.transform.position, playerData.superJumpFireKnockbackForce);
                col.TryGetComponent(out IDamageable damageable);
                damageable?.Damage(playerData.superJumpFireDamage, player.transform.position);
            }
        }

        if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
        {
            player.VFXController.ActivateSuperJumpLandWindVFX();
        }
        else
        {
            player.VFXController.ActivateSuperJumpLandFireVFX();
        }
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.superJumpEnergyCost);
    }
}
