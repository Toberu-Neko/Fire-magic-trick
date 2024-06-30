using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public float rotationSpeed = 15f;
    public float rotateSmoothTime = 0.1f;

    [Header("Move")]
    public float walkSpeed = 3f;
    public float slowRunSpeed = 5f;
    public float fastRunSpeed = 8f;
    public float aimMoveSpeed = 2f;
    public float airMoveSpeed = 3f;

    [Header("Jump")]
    public float jumpVelocity = 15f;
    public float jumpVelocityAfterSuperDash = 25f;
    public int amountOfJumps = 1;
    public float jumpInpusStopYSpeedMultiplier = 0.5f;
    public float coyoteTime = 0.2f;
    public float frameOfDecaySpeed = 0.75f;

    [Header("InAir")]
    public int maxFloatCount = 1;
    public float floatSpeed = -1f;
    public float floatHoldJumpTime = 0.2f;
    public float inAirMaxFloatTime = 3f;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    [Header("Super Dash")]
    public float baseSuperDashSpeed = 5f;
    public float maxSuperDashSpeed = 25f;
    public AnimationCurve superDashSpeedGraph;
    public float speedUpTime = 0.5f;
    public float maxSuperDashTime = 2f;
    public float superDashCooldown = 0.75f;
    public float afterSuperDashMultiplier = 0.65f;
    public float targetYOffset = 1f;

    public float superDashJumpTime = 0.5f;
    public float superDashJumpVelocity = 10f;
    public AnimationCurve superDashDecaySpeedGraph;

    [Header("Fireball")]
    public float fireballSpeed = 4f;
    public float fireballForceValue = 12.5f;
    public float fireballMaxYVelocity = -0.5f;
    public float fireballMinYVelocity = -2f;
    public float fireballMaxTime = 3f;
}
