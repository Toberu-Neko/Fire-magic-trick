using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaWaterBullet_Ready : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedTransform behaviorObject;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Duration")]
    [SerializeField] private float duration = 1f;

    private Transform waterBulletPoint; // 水球發射點
    private float timer; // 結束計時器

    public override void OnStart()
    {
        // 抓取水砲發射點
        waterBulletPoint = behaviorObject.Value.Find("WaterBulletPoint");

        // 有水砲Prefab與發射點
        if (bulletPrefab != null && waterBulletPoint != null) 
        {
            // 生成水砲蓄力
            GameObject bullet = Object.Instantiate(bulletPrefab, waterBulletPoint.position, waterBulletPoint.rotation);
        }

        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - timer >= duration)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {

    }
}