using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireAltState : PlayerAbilityState
{
    public PlayerFireAltState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = true;
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.fireAltEnergyCost);
    }
}
