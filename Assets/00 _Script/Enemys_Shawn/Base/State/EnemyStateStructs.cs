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
    [Tooltip("���m�ɶ�")]
    public float idleTime = 2f;
    [Tooltip("�̤p���m�ɶ�")]
    public float minIdleTime = 1f;
    [Tooltip("�̤j���m�ɶ�")]
    public float maxIdleTime = 3f;
}
public class ED_EnemyPatrol
{

}
public class ED_EnemyAlert
{

}
public class ED_EnemyAttack
{

}
public class ED_EnemyDeath
{

}
