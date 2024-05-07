using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class SohaMeleeRangeTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("梭哈行為樹")]public BehaviorTree behaviorTree;

    private void OnTriggerEnter(Collider other)
    {
        if (behaviorTree != null && other.CompareTag("Player"))
        {
            behaviorTree.SetVariableValue("playerInMeleeRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (behaviorTree != null && other.CompareTag("Player"))
        {
            behaviorTree.SetVariableValue("playerInMeleeRange", false);
        }
    }
}
