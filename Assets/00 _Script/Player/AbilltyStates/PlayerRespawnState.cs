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

        player.ChangeActiveCam(Player.ActiveCamera.DeterminBySpeed);
        player.SetCollider(true);
        player.SetModel(false);

        if(player.RespawnPosition != Vector3.zero)
        {
            player.Teleport(player.RespawnPosition);
        }

        player.VFXController.ActivateRespawnVFX();

        movement.SetGravityZero();

        stats.Health.Init();
        player.CardSystem.Init();
        UIManager.Instance.HudUI.SetBar(0.5f);
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
