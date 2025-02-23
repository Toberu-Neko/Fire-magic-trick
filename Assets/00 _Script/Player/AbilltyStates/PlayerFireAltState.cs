using UnityEngine;

public class PlayerFireAltState : PlayerAbilityState
{
    private float minYVelocity;
    private int currentFrame;
    private float startShootingTime;
    private bool firstTimeDrop;

    private float lastShootTime;
    public PlayerFireAltState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        lastShootTime = 0f;
        minYVelocity = Mathf.Infinity;
        currentFrame = 0;
        firstTimeDrop = true;
        startShootingTime = 0f;

        player.ChangeActiveCam(Player.ActiveCamera.Skill);
        player.InputHandler.UseSkillInput();
        player.CardSystem.DecreaseCardEnergy(playerData.altEnergyCost);

        player.SetColliderAndModel(false);
        player.VFXController.SetSuperDashVFX(true);
        player.VFXController.PlayFireExplode();
        BulletTimeManager.Instance.BulletTime_Slow(0.2f);

        movement.SetVelocityY(playerData.superJumpVelocity);

        foreach(var obj in SphereDetection(playerData.longRangeDetectRadius))
        {
            obj.TryGetComponent(out IFlammable flammable);
            flammable?.SetOnFire(5f);
        }

        UIManager.Instance.HudUI.HudVFX.FireAltEffect();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (currentFrame == 0)
        {
            movement.SetVelocityY(playerData.superJumpVelocity);
        }

        if(currentFrame > 5)
        {
            minYVelocity = Mathf.Min(minYVelocity, movement.CurrentVelocity.y);

            if(minYVelocity < -1f)
            {
                if (firstTimeDrop)
                {
                    firstTimeDrop = false;
                    startShootingTime = Time.time;
                    player.VFXController.PlayFireExplode();

                    foreach (var obj in SphereDetection(playerData.longRangeDetectRadius))
                    {
                        obj.TryGetComponent(out IFlammable flammable);
                        flammable?.SetOnFire(5f);
                    }

                    BulletTimeManager.Instance.BulletTime_Slow(0.2f);
                }

                movement.SetVelocityY(-0.5f);
                MoveWithInput(playerData.aimMoveSpeed, 0f, true);

                //shoot
                if(Time.time > lastShootTime + playerData.fireAltFireRate)
                {
                    lastShootTime = Time.time;

                    for (int i = 0; i < playerData.fireAltFireBullet; i++)
                    {
                        player.CardSystem.FireAltShoot();
                    }

                    AudioManager.Instance.PlayRandomSoundFX(player.CardSystem.ShootSFXs, player.transform, AudioManager.SoundType.twoD);
                }

                if (Time.time > startShootingTime + playerData.fireAltFireTime)
                {
                    isAbilityDone = true;
                }
            }
            else
            {
                MoveRelateWithCam(playerData.airMoveSpeed, 0f, true);
            }
        }

        currentFrame++;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(minYVelocity < -1f)
        {
            movement.RotateAdd(10f);
        }
        else
        {
            Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.ChangeActiveCam(Player.ActiveCamera.DeterminBySpeed);
        player.SetColliderAndModel(true);
        player.VFXController.SetSuperDashVFX(false);
    }
}
