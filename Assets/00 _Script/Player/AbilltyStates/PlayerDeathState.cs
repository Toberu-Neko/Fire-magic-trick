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
        player.SetCollider(false);
        player.SetPlayerModel(false);
        player.ChangeActiveCam(Player.ActiveCamera.Death);
        player.VFXController.ActivateDeathVFX();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > StartTime + playerData.deathAnimationTime)
        {
            player.PlayerDeath();
        }
    }
}
