using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWindAltState : PlayerAbilityState
{
    private List<DetectionInfo> detectionInfos;
    public float hitTime;
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

        player.CardSystem.DecreaseCardEnergy(playerData.windAltEnergyCost);
        player.SetCollider(false);
        player.SetPlayerModel(false);
        stats.SetInvincible(true);
        movement.SetGravityZero();
        player.VFXController.SetWindFeetCardVFX(true);

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

        Vector3 target = new(detectionInfos[0].transform.position.x, detectionInfos[0].transform.position.y + 1f, detectionInfos[0].transform.position.z);
        Vector3 targetDir = target - player.transform.position;
        float speed = playerData.windAltMaxSpeed * playerData.windAltSpeedCurve.Evaluate(Mathf.Clamp01((Time.time - StartTime) / playerData.windAltSpeedUpTime));
        movement.SetVelocity(speed, targetDir.normalized, true);

        if (Vector3.Distance(player.transform.position, target) < 0.5f)
        {
            detectionInfos[0].knockbackable.Knockback(targetDir.normalized, 10f, player.transform.position);
            detectionInfos[0].damageable.Damage(playerData.windAltDamage, player.transform.position);
            BulletTimeManager.Instance.BulletTime_Slow(0.2f);

            hitTime = Time.time;

            detectionInfos.RemoveAt(0);
        }

    }

    public override void Exit()
    {
        base.Exit();

        player.SetCollider(true);
        player.SetPlayerModel(true);
        stats.SetInvincible(false);
        movement.SetVelocityZero();
        movement.SetGravityOrginal();
        player.VFXController.SetWindFeetCardVFX(false);
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.windAltEnergyCost) && SphereDetection(playerData.longRangeDetectRadius).Count > 0;
    }
}
