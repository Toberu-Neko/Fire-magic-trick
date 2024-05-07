using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaSmash_Attack : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedTransform behaviorObject;

    [Header("Duration")]
    [SerializeField] private float duration;

    private GameObject smashCollider;
    private Rigidbody rb;
    private float timer;

    public override void OnStart()
    {
        if(behaviorObject.Value != null)
        {
            // 抓取碰撞體
            smashCollider = behaviorObject.Value.Find("HandCollider").gameObject;

            // 啟動碰撞體
            ColliderController(true);
        }

        // 開始技能持續時間計時
        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        // 持續時間結束關閉碰撞體
        if (Time.time - timer > duration)
        {
			ColliderController(false);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

	private void ColliderController(bool isActive) // 碰撞體控制器
    {
        if(smashCollider != null)
        {
            smashCollider.SetActive(isActive);
        }
    }

    public override void OnEnd()
    {
        
    }
}