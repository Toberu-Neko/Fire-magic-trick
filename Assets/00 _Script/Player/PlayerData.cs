using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public float rotationSpeed = 15f;
    public float rotateSmoothTime = 0.1f;

    [Header("Move")]
    [Tooltip("Movement")]
    public float moveSpeed = 3f;
    public float slowRunSpeed = 5f;
    public float fastRunSpeed = 8f;
    public float aimMoveSpeed = 2f;
    public float airMoveSpeed = 3f;

    [Header("Jump")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;
    public float jumpInpusStopYSpeedMultiplier = 0.5f;
    public float coyoteTime = 0.2f;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
}
