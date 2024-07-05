using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperJumpState : PlayerAbilityState
{
    private float minYVelocity;
    public PlayerSuperJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.DecreaseAmountOfJumpsLeft();
        player.InputHandler.UseSuperJumpInput();
        player.CardSystem.DecreaseCardEnergy(playerData.superJumpEnergyCost);
        movement.SetVelocityY(playerData.superJumpVelocity);
        minYVelocity = Mathf.Infinity;

        //TODO Diferent SuperJump Ability
        if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
        {
            // �_���|�E�l�ĤH�A�Y�a�A�@������
        }
        else
        {
            // �_�h�ÿU�N�ĤH
        }

        isAbilityDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        minYVelocity = Mathf.Min(minYVelocity, movement.CurrentVelocity.y);

        if (minYVelocity < 0)
        {
            movement.AddForce(playerData.superJumpFallAddForce, Vector3.down);
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.superJumpEnergyCost);
    }
}
