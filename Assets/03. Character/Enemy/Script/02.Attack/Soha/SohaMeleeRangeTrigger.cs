using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class SohaMeleeRangeTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("梭哈行為樹")] BehaviorTree behaviorTree;
    [SerializeField, Tooltip("近戰疲勞時間(秒)")] float fatigueTime;
    float fatigueTimer;
    bool playerInMeleeRange;

    private void OnTriggerEnter(Collider other)
    {
        // 進入時設定為玩家近距離
        if (behaviorTree != null && other.CompareTag("Player"))
        {   
            playerInMeleeRange = true;
            behaviorTree.SetVariableValue("playerInMeleeRange", true);
        }

        //
        fatigueTimer = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        // 退出時設定為玩家遠距離
        if (behaviorTree != null && other.CompareTag("Player"))
        {
            playerInMeleeRange = false;
            behaviorTree.SetVariableValue("playerInMeleeRange", false);
        }
    }

    void Update()
    {
        if(behaviorTree != null) // 行為樹確認
        {
            // 近戰疲勞事件
            if(Time.time - fatigueTimer >= fatigueTime && playerInMeleeRange)
            {
                // 發送事件
                behaviorTree.SendEvent("meleeFatigue");

                // 重置疲勞計時
                fatigueTimer = Time.time;
            }
        }
    }
}
