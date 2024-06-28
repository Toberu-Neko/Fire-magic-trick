using System;
using UnityEngine;

[Serializable]
public class ED_EnemyIdleState
{
    [Header("Random Idel Time")]
    public bool useRandomIdleTime = false;
    public float IdelTime_Max;
    public float IdelTime_Min;
    [Header("Value")]
    [Tooltip("閒置時間")]
    public float idleTime = 2f;
    [Tooltip("最小閒置時間")]
    public float minIdleTime = 1f;
    [Tooltip("最大閒置時間")]
    public float maxIdleTime = 3f;
}
[Serializable]
public class ED_EnemyPatrol
{
    public float PatrolSpeed = 4f;
}
[Serializable]
public class ED_EnemyAlert
{

}
[Serializable]
public class ED_EnemyAttack
{

}
[Serializable]
public class ED_EnemyDeath
{

}
