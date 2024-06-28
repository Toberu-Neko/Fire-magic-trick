using UnityEngine;

public class Enemy_A : Entity
{
    //Core Components for Enemy
    protected EnemyNavigation navigation;

    public override void Awake()
    {
        base.Awake();
        navigation = Core.GetCoreComponent<EnemyNavigation>();
    }
}
