using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaLazer_Shoot : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedInt waterBulletCount;
    [SerializeField] private SharedGameObject soha;

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 80;

    [Header("Duration")]
    [SerializeField] private float duration = 3f;

    private float timer; // 結束計時器
    private Transform lazerC;
    private Transform lazerCollider;

    public override void OnStart()
    {
        // 抓取雷射
        lazerC = behaviorObject.Value.Find("VFX_C_Lazer");
        lazerCollider = behaviorObject.Value.Find("Lazer_Collider");

        // 有雷射Prefab與發射點
        if (lazerC != null) 
        {
            lazerC.gameObject.SetActive(true);
            lazerCollider.gameObject.SetActive(true);
            lazerC.GetComponent<ParticleSystem>().Play();
        }

        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        RotateToTarget();
        if(Time.time - timer >= duration)
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
        lazerC.gameObject.SetActive(false);
        lazerCollider.gameObject.SetActive(false);

        // 減掉水球使用次數
        waterBulletCount.Value -= 7;

        // 動畫
        soha.Value.GetComponent<Animator>().SetBool("isLazer",false);
    }
}