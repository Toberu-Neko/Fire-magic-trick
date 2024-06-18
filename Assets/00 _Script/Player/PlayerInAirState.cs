using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerFSMBaseState
{
    private bool jumpInput;
    private bool jumpInputStop;
    private bool dashInput;

    private float minYVelocity;
    private float maxYVelocity;

    private bool isGrounded;
    public bool IsJumping { get; private set; }
    private bool coyoteTime;

    private Vector3 v3Workspace;
    private Vector2 v2Workspace;
    private float inAirMovementSpeed;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        isGrounded = false;
        IsJumping = false;
        coyoteTime = false;
        inAirMovementSpeed = playerData.airMoveSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        v3Workspace = new Vector3();
        v2Workspace = new Vector2();

        if(movement.CurrentVelocityXZMagnitude > playerData.airMoveSpeed)
        {
            SetAirControlSpeed(movement.CurrentVelocityXZMagnitude);

            player.ChangeActiveCam(Player.ActiveCamera.Run);
        }
        else
        {
            SetAirControlSpeed(playerData.airMoveSpeed);

            player.ChangeActiveCam(Player.ActiveCamera.Normal);
        }
    }

    public override void Exit()
    {
        base.Exit();

        minYVelocity = Mathf.Infinity;
        maxYVelocity = Mathf.NegativeInfinity;
        isGrounded = false;
        IsJumping = false;
        coyoteTime = false;
        inAirMovementSpeed = playerData.airMoveSpeed;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (collisionSenses)
        {
            isGrounded = collisionSenses.Ground;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        if (minYVelocity > movement.CurrentVelocity.y)
        {
            minYVelocity = movement.CurrentVelocity.y;
        }
        if (maxYVelocity < movement.CurrentVelocity.y)
        {
            maxYVelocity = movement.CurrentVelocity.y;
        }



        if (collisionSenses.Ground && !IsJumping)
        {
            player.Anim.SetTrigger("land");

            if (inAirMovementSpeed > playerData.moveSpeed && MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.RunningState);
            }
            else if (MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.WalkState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else if (player.JumpState.CanJump() && jumpInput)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (dashInput && player.DashState.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            MoveAndRotateWithCam(inAirMovementSpeed, 0f, true);
        }
    }

    private void CheckJumpMultiplier()
    {
        if (IsJumping)
        {
            if (jumpInputStop)
            {
                movement.SetVelocityY(movement.CurrentVelocity.y * playerData.jumpInpusStopYSpeedMultiplier);

                IsJumping = false;
            }
            else if (minYVelocity < -1f)
            {
                IsJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time >= StartTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void SetIsJumping()
    {
        IsJumping = true;
    }
    public void StartCoyoteTime()
    {
        coyoteTime = true;
    }

    public void SetAirControlSpeed(float speed)
    {
        inAirMovementSpeed = speed;
    }
}
