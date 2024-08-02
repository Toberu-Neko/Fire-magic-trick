using UnityEngine;

public class FireCard : Bullet
{
    [Header("FireCard")]
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private float moveTime;
    [SerializeField] private float ThroughDistance;

    //variable
    private float timer;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        TimerSystem();
    }

    private void TimerSystem()
    {
        timer += Time.deltaTime;

        if (timer>moveTime)
        {
            ToStop();
        }
    }

    protected override void OnHitEnemy()
    {
        base.OnHitEnemy();
    }

    protected override void OnHitSomething()
    {
        base.OnHitSomething();

        ObjectPoolManager.SpawnObject(fireBallPrefab, transform.position, Quaternion.identity);
    }
    private void ToStop()
    {
        speed = 0;
    }
}
