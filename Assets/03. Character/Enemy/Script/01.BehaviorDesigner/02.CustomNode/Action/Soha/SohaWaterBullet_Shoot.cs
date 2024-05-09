using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaWaterBullet_Shoot : Action
{
    enum ShootingMode{Single,Double,Triple}

    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedInt WaterBulletCount;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ShootingMode shootingMode;

    [Header("Player")]
    [SerializeField] private float playerHeight = 0.3f;

    private Transform waterBulletPoint; // 水球發射點

    public override void OnStart()
    {
        waterBulletPoint = behaviorObject.Value.Find("WaterBulletPoint");
    }

    public override TaskStatus OnUpdate()
    {
        if (bulletPrefab != null && waterBulletPoint != null) // 有水砲Prefab與發射點
        {
            // 生成水砲
            switch(shootingMode)
            {
                case ShootingMode.Single:
                Vector3 targetPosition = new Vector3(targetObject.Value.transform.position.x, targetObject.Value.transform.position.y + playerHeight, targetObject.Value.transform.position.z);
                waterBulletPoint.LookAt(targetPosition);
                GameObject bullet = Object.Instantiate(bulletPrefab, waterBulletPoint.position, waterBulletPoint.rotation);
                WaterBulletCount.Value += 1;
                break;
            }
        }
        return TaskStatus.Success;
    }

    public override void OnEnd()
    {

    }
}