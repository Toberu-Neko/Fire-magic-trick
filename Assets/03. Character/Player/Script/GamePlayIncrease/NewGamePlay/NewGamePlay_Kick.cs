using UnityEngine;

public class NewGamePlay_Kick : NewGamePlay_Basic_Kick
{
    [Header("kick")]
    [SerializeField] private float bulletKeepTime;

    //Script
    private BulletTimeManager bulletTime;

    protected override void Start()
    {
        base.Start();

        bulletTime =  GameManager.Instance.GetComponent<BulletTimeManager>();
    }
    protected override void Onkick()
    {
        base.Onkick();
        bulletTime.BulletTime_Slow(bulletKeepTime);
        Debug.Log("Kick Kick Kick");
    }
}
