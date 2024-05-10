using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaSmash_Attack : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedGameObject soha;

    [Header("Duration")]
    [SerializeField] private float duration;

    private GameObject smashColliderL;
    private GameObject smashColliderR;
    private Rigidbody rb;
    private float timer;

    public override void OnStart()
    {
        if(behaviorObject.Value != null)
        {
            // 抓取碰撞體
            smashColliderL = behaviorObject.Value.Find("HandColliderL").gameObject;
            smashColliderR = behaviorObject.Value.Find("HandColliderR").gameObject;

            // 啟動碰撞體
            ColliderController(true);
        }

        // 開始技能持續時間計時
        timer = Time.time;

        // 動畫
        soha.Value.GetComponent<Animator>().SetBool("isBang",false);
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
        if(smashColliderL != null && smashColliderR != null)
        {
            smashColliderL.SetActive(isActive);
            smashColliderR.SetActive(isActive);
        }
    }

    public override void OnEnd()
    {
        
    }
}