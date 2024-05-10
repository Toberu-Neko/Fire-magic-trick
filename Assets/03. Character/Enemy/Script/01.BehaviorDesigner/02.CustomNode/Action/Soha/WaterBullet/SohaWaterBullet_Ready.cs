using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaWaterBullet_Ready : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedGameObject soha;

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 120;

    [Header("Duration")]
    [SerializeField] private float duration = 1f;

    private float timer; // 結束計時器
    private Transform bulletCharge;

    public override void OnStart()
    {
        bulletCharge = behaviorObject.Value.Find("SohaWaterBullet_Ready");

        // 播放水砲蓄力
        if (bulletCharge != null) 
        {
            bulletCharge.gameObject.SetActive(true);
            bulletCharge.GetComponent<ParticleSystem>().Play();
        }

        // 動畫
        soha.Value.GetComponent<Animator>().SetBool("isWaterBullet",true);

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
        Vector3 targetPosition = new Vector3(targetObject.Value.transform.position.x, transform.position.y, targetObject.Value.transform.position.z);
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
        bulletCharge.GetComponent<ParticleSystem>().Stop();
        bulletCharge.gameObject.SetActive(false);
    }
}