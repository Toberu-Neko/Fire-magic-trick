using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnState : PlayerAbilityState
{
    public PlayerRespawnState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stats.SetInvincible(true);

        player.TeleportToSavepoint();
        player.SetCollider(false);
        player.SetPlayerModel(false);
        player.ChangeActiveCam(Player.ActiveCamera.Normal);
        player.VFXController.ActivateRespawnVFX();

        movement.SetGravityZero();

        stats.Health.Init();
        player.CardSystem.Init();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement.SetVelocityZero();

        if (Time.time > StartTime + playerData.respawnTime)
        {
            isAbilityDone = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        movement.SetGravityOrginal();
        stats.SetInvincible(false);
        player.SetCollider(true);
        player.SetPlayerModel(true);
    }
}
