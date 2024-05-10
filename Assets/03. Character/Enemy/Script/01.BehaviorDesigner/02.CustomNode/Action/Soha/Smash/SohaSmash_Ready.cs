using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaSmash_Ready : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedGameObject soha;

    [Header("Duration")]
    [SerializeField] private float duration;

    private float timer;

    public override void OnStart()
    {
        // 開始技能持續時間計時
        timer = Time.time;

        // 動畫
        soha.Value.GetComponent<Animator>().SetBool("isBang",true);
    }

    public override TaskStatus OnUpdate()
    {
        // 持續時間結束關閉碰撞體
        if (Time.time - timer > duration)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        
    }
}