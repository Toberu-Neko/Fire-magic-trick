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

        if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
        {
            player.VFXController.ActivateWindStartVFX();

            // �_���|�E�l�ĤH
            foreach (var col in SphereDetection(playerData.longRangeDetectRadius))
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
            player.VFXController.ActivateFireStartVFX();

            // �_�h�ÿU�N�ĤH
            foreach (var col in SphereDetection(playerData.midRangeDetectRadius))
            {
                if (col != null)
                {
                    col.TryGetComponent(out IKnockbackable knockbackable);
                    knockbackable?.Knockback(player.transform.position, playerData.superJumpFireJumpKnockbackForce);
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

        if (player.InputHandler.JumpInput)
        {
            isAbilityDone = true;
        }

    }

    public override void Exit()
    {
        base.Exit();

        if (collisionSenses.Ground || collisionSenses.Enemy)
        {
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

            if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
            {
                player.VFXController.ActivateWindLandVFX();
            }
            else
            {
                player.VFXController.ActivateFireLandVFX();
            }
        }
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.superJumpEnergyCost);
    }
}
