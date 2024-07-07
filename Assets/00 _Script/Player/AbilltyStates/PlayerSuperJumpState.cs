using UnityEngine;

public class PlayerSuperJumpState : PlayerAbilityState
{
    private float minYVelocity;
    private int currentFrame;
    private bool firstTimeDrop;
    public PlayerSuperJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        currentFrame = 0;
        firstTimeDrop = false;

        player.InputHandler.UseSuperJumpInput();
        player.CardSystem.DecreaseCardEnergy(playerData.superJumpEnergyCost);
        movement.SetVelocityY(playerData.superJumpVelocity);
        minYVelocity = Mathf.Infinity;

        //TODO Different SuperJump Ability
        if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
        {
            // �_���|�E�l�ĤH
        }
        else
        {
            // �_�h�ÿU�N�ĤH
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentFrame++;

        MoveAndRotateWithCam(playerData.airMoveSpeed);

        if(currentFrame > 5)
        {
            minYVelocity = Mathf.Min(minYVelocity, movement.CurrentVelocity.y);

            if (minYVelocity < -1f)
            {
                if (!firstTimeDrop)
                {
                    firstTimeDrop = true;
                    movement.SetVelocityY(playerData.superJumpFallInitVelocity);
                }

                movement.AddForce(playerData.superJumpFallAddForce, Vector3.down);

                if (collisionSenses.Ground)
                {
                    //TODO Different SuperJump Ability
                    if (player.CardSystem.CurrentEquipedCard == CardSystem.CardType.Wind)
                    {
                        // �_���|�E�l�ĤH
                    }
                    else
                    {
                        // ��j���_�h�ÿU�N�ĤH
                    }
                    isAbilityDone = true;
                }
            }
        }
        else
        {
            movement.SetVelocityY(playerData.superJumpVelocity);
        }

    }

    public bool CanUseAbility()
    {
        return player.CardSystem.CheckCardEnergy(playerData.superJumpEnergyCost);
    }
}
