using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("MoveState")]
    [Tooltip("²¾°Ê³t«×")]
    public float movementVelocity = 10f;
}
