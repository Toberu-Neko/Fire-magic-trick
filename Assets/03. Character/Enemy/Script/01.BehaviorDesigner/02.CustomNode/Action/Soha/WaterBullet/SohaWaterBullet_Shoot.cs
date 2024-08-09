using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SohaWaterBullet_Shoot : Action
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject targetObject;
    [SerializeField] private SharedTransform behaviorObject;
    [SerializeField] private SharedInt waterBulletCount;
    [SerializeField] private SharedGameObject soha;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount;
    [SerializeField] private float offsetRange;
    [SerializeField] private float delayTime;

    [Header("Player")]
    [SerializeField] private float playerHeight = 0.3f;

    private Transform waterBulletPoint; // 水球發射點
    private float lastShootTime; // 上次射擊時間
    private int bulletAlreadyShoot; // 已射擊次數

    public override void OnStart()
    {
        // 還沒射完......
        lastShootTime = Time.time - lastShootTime;
        bulletAlreadyShoot = 0;

        // 抓取水砲發射點
        waterBulletPoint = behaviorObject.Value.Find("WaterBulletPoint");

        // 動畫
        soha.Value.GetComponent<Animator>().SetTrigger("waterBulletReadyEnd");
    }

    public override TaskStatus OnUpdate()
    {
        // 開始發射
        StartShooting();

        // 若子彈全部發射完畢
        if(bulletAlreadyShoot >= bulletCount)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    private void StartShooting()
    {
        // 如果間隔時間抵達
        if (Time.time - lastShootTime >= delayTime)
        {
            // 射擊
            ShootingWaterBullet();

            // 更新上次射擊時間
            lastShootTime = Time.time;

            // 射擊次數+1
            bulletAlreadyShoot += 1;
        }
    }
    void ShootingWaterBullet() // 發射水砲
    {
        // 目標點
        Vector3 targetPosition = new Vector3(GameManager.Instance.Player.position.x, GameManager.Instance.Player.position.y + playerHeight, GameManager.Instance.Player.position.z);
        
        // 目標點偏移
        Vector3 randomTargetPosition = new Vector3(
        targetPosition.x + Random.Range(-offsetRange, offsetRange),
        targetPosition.y + Random.Range(-offsetRange, offsetRange),
        targetPosition.z + Random.Range(-offsetRange, offsetRange)
        );

        // 設定瞄準目標
        waterBulletPoint.LookAt(randomTargetPosition);

        // 生成水砲
        GameObject bullet = Object.Instantiate(bulletPrefab, waterBulletPoint.position, waterBulletPoint.rotation);

        // 發射次數計數器
        waterBulletCount.Value += 1;
    }

    public override void OnEnd()
    {
        // 動畫
        soha.Value.GetComponent<Animator>().SetBool("isWaterBullet",false);
    }
}