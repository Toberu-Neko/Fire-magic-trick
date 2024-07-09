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
            isAbilityDone = true;
            return;
        }

        if (Time.time > hitTime + playerData.windAltMaxTime)
        {
            isAbilityDone = true;
            return;
        }

        Vector3 targetDir = detectionInfos[0].transform.position - player.transform.position;
        movement.SetVelocity(12f, targetDir.normalized, true);

        if (Vector3.Distance(player.transform.position, detectionInfos[0].transform.position) < 0.05f)
        {
            detectionInfos[0].knockbackable.Knockback(targetDir.normalized, 10f, player.transform.position);
            detectionInfos[0].damageable.Damage(playerData.windAltDamage, player.transform.position);
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
        movement.SetGravityOrginal();
        player.VFXController.SetWindFeetCardVFX(false);
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.windAltEnergyCost) && SphereDetection(playerData.longRangeDetectRadius).Count > 0;
    }
}
