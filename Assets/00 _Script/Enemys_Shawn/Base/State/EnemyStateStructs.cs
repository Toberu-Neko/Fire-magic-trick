using System;
using UnityEngine;

[Serializable]
public class ED_EnemyIdleState
{
    [Tooltip("最小閒置時間")]
    public float minIdleTime = 1f;
    [Tooltip("最大閒置時間")]
    public float maxIdleTime = 3f;
}
