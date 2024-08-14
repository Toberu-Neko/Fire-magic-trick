using UnityEngine;

public class PlayerSuperJumpState : PlayerAbilityState
{
    private float minYVelocity;
    private int currentFrame;
    private bool firstTimeDrop;

    private bool doDropDamage;
    public PlayerSuperJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        currentFrame = 0;
        firstTimeDrop = false;
        doDropDamage = true;

        player.ChangeActiveCam(Player.ActiveCamera.Skill);
        player.InputHandler.UseSuperJumpInput();
        player.JumpState.DecreaseAmountOfJumpsLeft();
        player.CardSystem.DecreaseCardEnergy(playerData.superJumpEnergyCost);
        movement.SetCanSetVelocity(true);
        movement.SetVelocityY(playerData.superJumpVelocity);
        minYVelocity = Mathf.Infinity;

        player.VFXController.SetCanComboVFX(player.InAirState.CheckCanFloat());
        AudioManager.Instance.PlaySoundFX(playerData.superJumpStartSound, player.transform, AudioManager.SoundType.twoD);

        if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
        {
            player.VFXController.PlayWindSuperJump();

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
            player.VFXController.PlayFireSuperJump();

            // �_�h�ÿU�N�ĤH
            foreach (var col in SphereDetection(playerData.midRangeDetectRadius))
            {
                if (col != null)
                {
                    col.TryGetComponent(out IKnockbackable knockbackable);
                    knockbackable?.Knockback(player.transform.position, playerData.superJumpFireJumpKnockbackForce);
                    col.TryGetComponent(out IDamageable damageable);
                    damageable?.Damage(playerData.superJumpFireDamage, player.transform.position);
                    col.TryGetComponent(out IFlammable flammable);
                    flammable?.SetOnFire(playerData.superJumpBurnTime);
                }
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentFrame++;

        MoveRelateWithCam(playerData.airMoveSpeed);

        if(currentFrame > 4)
        {
            minYVelocity = Mathf.Min(minYVelocity, movement.CurrentVelocity.y);

            if (minYVelocity < -1f)
            {
                if (!firstTimeDrop)
                {
                    firstTimeDrop = true;
                    BulletTimeManager.Instance.BulletTime_Slow(0.2f);
                    movement.SetVelocityY(playerData.superJumpFallStartVelocity);
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

        if (player.InputHandler.JumpInput && player.InAirState.CheckCanFloat())
        {
            doDropDamage = false;
            isAbilityDone = true;
        }
        else if (player.InputHandler.FireTransferInput && player.SuperDashState.CanSuperDash())
        {
            doDropDamage = false;
            player.SuperDashState.SetTarget(player.CardSystem.SuperDashTarget);
            stateMachine.ChangeState(player.SuperDashState);
        }
        else if(player.InputHandler.DashInput && player.DashState.CanDash())
        {
            doDropDamage = false;
            stateMachine.ChangeState(player.DashState);
        }

        if(Time.time >= StartTime + 2.5f)
        {
            player.ChangeActiveCam(Player.ActiveCamera.DeterminBySpeed);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);
    }

    public override void Exit()
    {
        base.Exit();

        player.ChangeActiveCam(Player.ActiveCamera.DeterminBySpeed);
        if (doDropDamage)
        {
            AudioManager.Instance.PlaySoundFX(playerData.superJumpLandSound, player.transform, AudioManager.SoundType.twoD);
            foreach (var col in SphereDetection(playerData.longRangeDetectRadius))
            {
                if (col != null)
                {
                    col.TryGetComponent(out IKnockbackable knockbackable);
                    knockbackable?.Knockback(player.transform.position, playerData.superJumpLandKnockbackForce);
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

        player.VFXController.SetCanComboVFX(false);
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.superJumpEnergyCost);
    }
}
