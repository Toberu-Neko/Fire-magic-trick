using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaLazer_Ready : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject ringPrefab;

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 120;

    [Header("Duration")]
    [SerializeField] private float chargeDuration = 3f;
    [SerializeField] private float ringDuration = 1f;

    private Transform lazerPoint; // 雷射發射點
    private float timer; // 結束計時器
    private bool hasRing;

    public override void OnStart()
    {
        hasRing = false;

        // 抓取雷射發射點
        lazerPoint = behaviorObject.Value.Find("LazerPoint");

        // 有雷射Prefab與發射點
        if (bulletPrefab != null && lazerPoint != null) 
        {
            // 生成雷射蓄力
            GameObject bullet = Object.Instantiate(bulletPrefab, lazerPoint.position, lazerPoint.rotation);

            // 設定父物件
            bullet.transform.SetParent(lazerPoint);
        }

        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        RotateToTarget();
        if(Time.time - timer >= chargeDuration - ringDuration && !hasRing)
        {
            // 生成雷射蓄力
            GameObject ring = Object.Instantiate(ringPrefab, lazerPoint.position, lazerPoint.rotation);

            // 設定父物件
            ring.transform.SetParent(lazerPoint);

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

    }
}