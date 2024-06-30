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

    private float jumpStartTime;
    public bool IsJumping { get; private set; }
    private bool coyoteTime;

    private Vector3 v3Workspace;
    private Vector2 v2Workspace;
    private float inAirMovementSpeed;

    private bool setAirControlSpeed;

    private bool isFloating;
    private float startFloatingTime;
    private int floatCount;
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
        floatCount = 0;
        isFloating = false;
        startFloatingTime = 0f;
        jumpStartTime = 0f;

        v3Workspace = new Vector3();
        v2Workspace = new Vector2();

        if (!setAirControlSpeed)
        {
            if (movement.CurrentVelocityXZMagnitude > playerData.airMoveSpeed)
            {
                if (movement.CurrentVelocityXZMagnitude > playerData.dashSpeed)
                {
                    SetAirControlSpeed(playerData.dashSpeed);
                }
                else
                {
                    SetAirControlSpeed(movement.CurrentVelocityXZMagnitude);
                }


                player.ChangeActiveCam(Player.ActiveCamera.Run);
            }
            else
            {
                SetAirControlSpeed(playerData.airMoveSpeed);

                player.ChangeActiveCam(Player.ActiveCamera.Normal);
            }
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
        setAirControlSpeed = false;
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

        CheckIfShouldShoot();
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

        if (collisionSenses.Ground && !IsJumping && !collisionSenses.Slope.ExceedsMaxSlopeAngle)
        {
            player.Anim.SetTrigger("land");

            if (inAirMovementSpeed < playerData.slowRunSpeed && MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.WalkingState);
            }
            else if (MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.RunningState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else if (player.JumpState.CanJump() && jumpInput && Time.time - player.InputHandler.JumpInputStartTime < playerData.floatHoldJumpTime)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (dashInput && player.DashState.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (player.InputHandler.FireTransferInput && player.CardSystem.HasSuperDashTarget && player.SuperDashState.CanSuperDash())
        {
            player.SuperDashState.SetTarget(player.CardSystem.SuperDashTarget);
            stateMachine.ChangeState(player.SuperDashState);
        }
        else
        {
            MoveAndRotateWithCam(inAirMovementSpeed, 0f, true);

            if (!isFloating && floatCount < playerData.maxFloatCount && player.InputHandler.OrgJumpInput && minYVelocity < -1f)
            {
                player.InputHandler.UseJumpInput();
                isFloating = true;
                startFloatingTime = Time.time;
            }
            else if (isFloating && (!player.InputHandler.OrgJumpInput || Time.time - startFloatingTime > playerData.inAirMaxFloatTime))
            {
                isFloating = false;
                floatCount++;
            }

            if (isFloating)
            {
                movement.SetVelocityY(playerData.floatSpeed);
            }

            inAirMovementSpeed = Mathf.Lerp(inAirMovementSpeed, playerData.airMoveSpeed, playerData.frameOfDecaySpeed * Time.deltaTime);

            /*
            if (!IsJumping && collisionSenses.Slope.IsOnSlope && collisionSenses.Slope.ExceedsMaxSlopeAngle)
            {
                Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);
                movement.SetVelocityY(Mathf.Lerp(movement.CurrentVelocity.y, -playerData.slideDownSlopeSpeed, 2f * Time.deltaTime));
            }
            else
            {
                
            }
            */
        }

    }

    private void CheckJumpMultiplier()
    {
        if (IsJumping)
        {
            if (jumpInputStop && Time.time - jumpStartTime > 3f * Time.deltaTime)
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
        jumpStartTime = Time.time;

        IsJumping = true;
    }
    public void StartCoyoteTime()
    {
        coyoteTime = true;
    }

    public void SetAirControlSpeed(float speed)
    {
        inAirMovementSpeed = speed;
        setAirControlSpeed = true;
    }
}
