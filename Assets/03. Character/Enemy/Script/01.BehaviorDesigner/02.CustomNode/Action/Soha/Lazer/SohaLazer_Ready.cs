using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaLazer_Ready : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedGameObject soha;

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 120;

    [Header("Duration")]
    [SerializeField] private float chargeDuration = 3f;
    [SerializeField] private float ringDuration = 1f;

    private float timer; // 結束計時器
    private bool hasRing;

    private Transform lazerA;
    private Transform lazerB;

    public override void OnStart()
    {
        hasRing = false;

        // 抓取雷射
        lazerA = behaviorObject.Value.Find("VFX_A_LazerPowerCharge");
        lazerB = behaviorObject.Value.Find("VFX_B_LazerAttackCharge");

        // 有雷射Prefab與發射點
        if (lazerA != null) 
        {
            lazerA.gameObject.SetActive(true);
            lazerA.GetComponent<ParticleSystem>().Play();
        }

        // 動畫
        soha.Value.GetComponent<Animator>().SetBool("isLazer",true);

        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        RotateToTarget();
        if(Time.time - timer >= chargeDuration - ringDuration && !hasRing)
        {
            // 生成雷射蓄力
            lazerB.gameObject.SetActive(true);
            lazerB.GetComponent<ParticleSystem>().Play();

            // 動畫
            soha.Value.GetComponent<Animator>().SetTrigger("lazerReadyEnd");

            // 設定已生成過環
            hasRing = true;
        }
        if(Time.time - timer >= chargeDuration)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition = new Vector3(GameManager.Instance.Player.position.x, transform.position.y, GameManager.Instance.Player.position.z);
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

        float angle = Quaternion.Angle(transform.rotation, rotation);

        float maxRotationSpeed = rotateSpeed * Time.deltaTime;
        if (angle > maxRotationSpeed)
        {
            float t = maxRotationSpeed / angle;
            rotation = Quaternion.Slerp(transform.rotation, rotation, t);
        }
        transform.rotation = rotation;
    }

    public override void OnEnd()
    {
        // 關閉特效
        lazerA.gameObject.SetActive(false);
        lazerB.gameObject.SetActive(false);
    }
}