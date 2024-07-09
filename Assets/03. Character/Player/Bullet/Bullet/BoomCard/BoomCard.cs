using UnityEngine;

public class BoomCard : Bullet
{
    [Header("Boom Card")]
    [SerializeField] private GameObject boomArea;
    [SerializeField] private GameObject fireRetrun;
    protected override void OnHitEnemy()
    {
        base.OnHitEnemy();

        Instantiate(fireRetrun, transform.position, Quaternion.identity);
    }
    protected override void OnHitSomething()
    {
        base.OnHitSomething();

        Instantiate(boomArea, transform.position, Quaternion.identity);
    }
}
