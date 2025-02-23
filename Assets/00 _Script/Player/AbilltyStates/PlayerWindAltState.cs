using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWindAltState : PlayerAbilityState
{
    private List<DetectionInfo> detectionInfos;
    public float hitTime;
    private Vector3 startPos;
    private int attackCount;
    private bool gotoOrgPos;
    private float gotoOrgPosTime;

    private class DetectionInfo
    {
        public DetectionInfo(IDamageable damageable, IKnockbackable knockbackable, Transform pos, float distance)
        {
            this.damageable = damageable;
            this.knockbackable = knockbackable;
            this.transform = pos;
            this.distance = distance;
        }

        public IDamageable damageable;
        public IKnockbackable knockbackable;
        public Transform transform;
        public float distance;
    }
    public PlayerWindAltState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.CardSystem.DecreaseCardEnergy(playerData.altEnergyCost);
        player.SetColliderAndModel(false);
        movement.SetGravityZero();
        player.VFXController.SetWindFeetCardVFX(true);
        startPos = player.transform.position;
        player.ChangeActiveCam(Player.ActiveCamera.Skill);

        //Detect Enemy
        detectionInfos = new();
        foreach (var det in SphereDetection(playerData.longRangeDetectRadius))
        {
            det.TryGetComponent(out IDamageable damageable);
            det.TryGetComponent(out IKnockbackable knockbackable);

            if(damageable != null && knockbackable != null)
            {
                detectionInfos.Add(new DetectionInfo(damageable, knockbackable, det.transform, Vector3.Distance(player.transform.position, det.transform.position)));
            }
        }

        //Sort by distance
        detectionInfos.Sort((a, b) => a.distance.CompareTo(b.distance));

        hitTime = Time.time;
        attackCount = 0;

        UIManager.Instance.HudUI.HudVFX.WindAltEffect();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (detectionInfos.Count == 0)
        {
            stateMachine.ChangeState(player.AfterSuperDashJump);
            return;
        }

        if (Time.time > hitTime + playerData.windAltMaxTime)
        {
            stateMachine.ChangeState(player.AfterSuperDashJump);
            return;
        }

        if (!gotoOrgPos)
        {

            Vector3 target = new(detectionInfos[0].transform.position.x, detectionInfos[0].transform.position.y + 1f, detectionInfos[0].transform.position.z);
            Vector3 targetDir = target - player.transform.position;
            float speed = playerData.windAltMaxSpeed * playerData.windAltSpeedCurve.Evaluate(Mathf.Clamp01((Time.time - StartTime) / playerData.windAltSpeedUpTime));
            movement.SetVelocity(speed, targetDir.normalized, true);

            if (Vector3.Distance(player.transform.position, target) < 0.5f)
            {
                detectionInfos[0].knockbackable.Knockback(targetDir.normalized, 10f, player.transform.position);
                detectionInfos[0].damageable.Damage(playerData.windAltDamage, player.transform.position);
                BulletTimeManager.Instance.BulletTime_Slow(playerData.windBulletTimeDuration);

                hitTime = Time.time;
                attackCount++;

                if (detectionInfos.Count == 1 && attackCount < 3)
                {
                    gotoOrgPos = true;
                    gotoOrgPosTime = Time.time;
                }
                else
                {
                    detectionInfos.RemoveAt(0);
                }
            }
        }
        else
        {
            if (Time.time > gotoOrgPosTime + 0.3f)
            {
                gotoOrgPos = false;
            }
            else
            {
                Vector3 targetDir = startPos - player.transform.position;
                float speed = playerData.windAltMaxSpeed * playerData.windAltSpeedCurve.Evaluate(Mathf.Clamp01((Time.time - StartTime) / playerData.windAltSpeedUpTime));
                movement.SetVelocity(speed, targetDir.normalized, true);
            }

        }
    }

    public override void Exit()
    {
        base.Exit();

        player.ChangeActiveCam(Player.ActiveCamera.DeterminBySpeed);
        player.SetColliderAndModel(true);
        movement.SetVelocityZero();
        movement.SetGravityOrginal();
        player.VFXController.SetWindFeetCardVFX(false);
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.altEnergyCost) && SphereDetection(playerData.longRangeDetectRadius).Count > 0;
    }
}
