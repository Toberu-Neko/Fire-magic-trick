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

        player.TeleportToSavepoint();
        player.SetColliderAndModel(false);
        player.ChangeActiveCam(Player.ActiveCamera.Normal);
        player.VFXController.ActivateRespawnVFX();

        movement.SetGravityZero();

        stats.Health.Init();
        player.CardSystem.Init();
        GameManager.Instance.PlayerReborn();
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
        player.SetColliderAndModel(true);
    }
}
