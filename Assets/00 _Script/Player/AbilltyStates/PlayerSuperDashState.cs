using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperDashState : PlayerAbilityState
{
    private Transform target;
    private Vector3 targetVector;
    private Vector3 targetPos;

    private bool jumpInput;
    private bool goToAirJumpState;

    public PlayerSuperDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public override void Enter()
    {
        base.Enter();

        if(target == null)
        {
            Debug.LogError("Target is null");
            isAbilityDone = true;
        }

        movement.SetVelocityZero();
        targetPos.Set(target.position.x, target.transform.position.y + playerData.targetYOffset, target.position.z);
        targetVector = targetPos - player.transform.position;
        targetVector.Normalize();
        goToAirJumpState = false;

        stats.Health.Increase(playerData.superDashEnergyCost);

        player.SetColliderAndModel(false);
        player.VFXController.SetSuperDashVFX(true);

        UIManager.Instance.HudUI.HudVFX.SuperDashSpeedLineEffect(true);
        player.SetSuperDashAudio(true);
    }

    public override void Exit()
    {
        base.Exit();

        target = null;
        player.SetColliderAndModel(true);
        player.VFXController.SetSuperDashVFX(false);
        player.VFXController.SuperDashHit();

        player.DoDashHitImpluse(1.25f);
        BulletTimeManager.Instance.BulletTime_Slow(0.25f);

        List<GameObject> detectedobj = SphereDetection(playerData.closeRangeDetectRadius);

        if (detectedobj.Count > 0)
        {
            player.VFXController.ActivateFireLandVFX();
            foreach (GameObject col in detectedobj)
            {
                if (col.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(playerData.superDashDamage, player.transform.position);
                }

                if (col.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.Knockback(player.transform.position, playerData.superDashKnockbackForce);
                }

                if (col.TryGetComponent(out IFlammable flammable))
                {
                    flammable.SetOnFire(playerData.superDashBurnTime);
                }
            }
        }

        UIManager.Instance.HudUI.HudVFX.SuperDashSpeedLineEffect(false);
        AudioManager.Instance.PlaySoundFX(playerData.superDashHitSFX, player.transform, AudioManager.SoundType.twoD);
        player.SetSuperDashAudio(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = player.InputHandler.JumpInput;

        if(target == null)
        {
            DetermineNextState();
            Debug.LogError("Target is null in Super Dash State");
            return;
        }

        targetPos.Set(target.position.x, target.transform.position.y + playerData.targetYOffset, target.position.z);
        if (jumpInput)
        {
            player.InputHandler.UseJumpInput();
            goToAirJumpState = true;
        }

        if (target == null)
        {
            DetermineNextState();
            return;
        }

        if (Vector3.Distance(player.transform.position, targetPos) <= 0.5f)
        {
            UIManager.Instance.HudUI.HudVFX.SuperDashHitSpeedLineEffect();
            DetermineNextState();
            return;
        }
        else if(Time.time >= StartTime + playerData.maxSuperDashTime)
        {
            DetermineNextState();
            return;
        }
        else
        {
            targetVector = targetPos - player.transform.position;
            targetVector.Normalize();
            float speed = playerData.maxSuperDashSpeed * playerData.superDashSpeedGraph.Evaluate(Mathf.Clamp01((Time.time - StartTime) / playerData.speedUpTime));
            movement.SetVelocity(speed, targetVector);
        }
    }

    public bool CanSuperDash()
    {
        return Time.time >= ExitTime + playerData.superDashCooldown && player.CardSystem.HasSuperDashTarget && stats.Health.GapBetweenCurrentAndMax >= playerData.superDashEnergyCost;
    }

    private void DetermineNextState()
    {
        if (goToAirJumpState || collisionSenses.OrgPointGround)
        {
            movement.SetVelocityZero();
            stateMachine.ChangeState(player.AfterSuperDashJump);
        }
        else
        {
            movement.SetVelocity(movement.RB.velocity.magnitude * playerData.afterSuperDashMultiplier, targetVector);
            stateMachine.ChangeState(player.FireballState);
        }
    }
}
