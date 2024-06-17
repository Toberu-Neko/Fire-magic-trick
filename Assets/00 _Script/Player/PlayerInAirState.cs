using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerFSMBaseState
{
    private Vector2 movementInput;
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
        coyoteTime = true;
        inAirMovementSpeed = playerData.airMoveSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        v3Workspace = new Vector3();
        v2Workspace = new Vector2();

    }

    public override void Exit()
    {
        base.Exit();

        minYVelocity = Mathf.Infinity;
        maxYVelocity = Mathf.NegativeInfinity;
        isGrounded = false;
        IsJumping = false;
        coyoteTime = true;
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

        movementInput = player.InputHandler.RawMovementInput;
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
            if(inAirMovementSpeed > playerData.airMoveSpeed && movementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.RunningState);
            }
            else if (movementInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.WalkState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            v3Workspace.Set(movementInput.x, 0f, movementInput.y);

            if (v3Workspace.magnitude > 1f)
            {
                v3Workspace.Normalize();
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            if (v3Workspace.magnitude != 0f)
            {
                Rotate(playerData.rotationSpeed, playerData.rotateSmoothTime);

                float speed = inAirMovementSpeed * v3Workspace.magnitude;
                v2Workspace.Set(targetDirection.x, targetDirection.z);

                Move(speed, v2Workspace, true);
            }
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
