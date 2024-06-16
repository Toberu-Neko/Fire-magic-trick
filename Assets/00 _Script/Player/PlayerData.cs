using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public float rotationSpeed = 15f;
    public float rotateSmoothTime = 0.1f;

    [Header("MoveState")]
    [Tooltip("²¾°Ê³t«×")]
    public float moveSpeed = 3f;
    public float slowRunSpeed = 5f;
    public float fastRunSpeed = 8f;
}
