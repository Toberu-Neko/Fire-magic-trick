using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWindAltState : PlayerAbilityState
{
    public PlayerWindAltState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.CardSystem.DecreaseCardEnergy(playerData.windAltEnergyCost);
        player.SetCollider(false);

        //Detect Enemy

    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.windAltEnergyCost);
    }
}
