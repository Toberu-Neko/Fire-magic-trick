using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerFSMBaseState
{
    public PlayerDeathState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stats.SetInvincible(true);
        movement.SetGravityZero();
        player.SetCollider(false);
        player.SetPlayerModel(false);
        player.ChangeActiveCam(Player.ActiveCamera.Death);
        player.VFXController.ActivateDeathVFX();

        //TODO: BlackUI
        UIManager.Instance.ActivateDeathUI();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement.SetVelocityZero();

        if(Time.time > StartTime + playerData.deathAnimationTime && UIManager.Instance.IsDeathUIOpenFinished())
        {
            stateMachine.ChangeState(player.RespawnState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        stats.SetInvincible(false);
        player.SetCollider(true);
        player.SetPlayerModel(true);
        movement.SetGravityOrginal();

        UIManager.Instance.DeactivateDeathUI();
    }
}
