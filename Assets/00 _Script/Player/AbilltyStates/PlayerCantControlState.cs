using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCantControlState : PlayerAbilityState
{
    public PlayerCantControlState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
}
