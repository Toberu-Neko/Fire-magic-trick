using MagicaCloth2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperDashState : PlayerAbilityState
{
    private Transform target;
    private Vector3 targetVector;
    private Vector3 targetPos;

    private bool jumpInput;
    private bool goToAirJumpState;

    public PlayerSuperDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public override void Enter()
    {
        base.Enter();

        if(target == null)
        {
            Debug.LogError("Target is null");
            isAbilityDone = true;
        }

        movement.SetVelocityZero();
        targetPos.Set(target.position.x, target.transform.position.y + playerData.targetYOffset, target.position.z);
        targetVector = targetPos - player.transform.position;
        targetVector.Normalize();
        goToAirJumpState = false;

        player.SetCollider(false);
        player.SetPlayerModel(false);
        player.VFXController.SetSuperDashVFX(true);
    }

    public override void Exit()
    {
        base.Exit();

        target = null;
        player.SetCollider(true);
        player.SetPlayerModel(true);
        player.VFXController.SetSuperDashVFX(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = player.InputHandler.JumpInput;

        targetPos.Set(target.position.x, target.transform.position.y + playerData.targetYOffset, target.position.z);
        if (jumpInput)
        {
            player.InputHandler.UseJumpInput();
            goToAirJumpState = true;
        }

        if (target == null)
        {
            DetermineNextState();
            return;
        }

        if (Vector3.Distance(player.transform.position, targetPos) <= 0.5f)
        {
            DetermineNextState();
            return;
        }
        else if(Time.time >= StartTime + playerData.maxSuperDashTime)
        {
            DetermineNextState();
            return;
        }
        else
        {
            targetVector = targetPos - player.transform.position;
            targetVector.Normalize();
            float speed = playerData.maxSuperDashSpeed * playerData.superDashSpeedGraph.Evaluate(Mathf.Clamp01((Time.time - StartTime) / playerData.speedUpTime));
            movement.SetVelocity(speed, targetVector);
        }
    }

    public bool CanSuperDash()
    {
        return Time.time >= ExitTime + playerData.superDashCooldown;
    }

    private void DetermineNextState()
    {
        if (goToAirJumpState)
        {
            movement.SetVelocityZero();
            stateMachine.ChangeState(player.AfterSuperDashJump);
        }
        else
        {
            movement.SetVelocity(movement.RB.velocity.magnitude * playerData.afterSuperDashMultiplier, targetVector);
            stateMachine.ChangeState(player.FireballState);
        }
    }
}
