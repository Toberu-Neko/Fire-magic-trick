using UnityEngine;

public class WindCard : Bullet
{
    [Header("WindCard")]
    [SerializeField] private GameObject windCardReturnPrefab;

    //Script
    private TrackSystem trackSystem;
    protected override void Start()
    {
        base.Start();

        //Script
        trackSystem = GetComponent<TrackSystem>();

        //Setting
        useTriggerEnter = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(trackSystem != null)
        {
            trackSystem.enabled = false;
        }
    }

    protected override void OnHitEnemy()
    {
        base.OnHitEnemy();

        ObjectPoolManager.SpawnObject(windCardReturnPrefab, transform.position, Quaternion.identity);
    }
    protected override bool NeedHitFeedback()
    {
        return false;
    }
    protected override void OnHitSomething()
    {
        base.OnHitSomething();
    }
}
