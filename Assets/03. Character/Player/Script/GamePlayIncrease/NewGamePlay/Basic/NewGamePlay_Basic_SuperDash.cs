using UnityEngine;

public class NewGamePlay_Basic_SuperDash : MonoBehaviour
{
    //Script
    protected SuperDash superDash;
    protected NewGamePlay_Combo combo;
    protected virtual void Start()
    {
        superDash = GameManager.Instance.EnergySystem.GetComponent<SuperDash>();
        combo = GetComponent<NewGamePlay_Combo>();

        superDash.OnSuperDashStart += OnSuperDashStart;
        superDash.OnSuperDashHitGround += OnSuperDashHitGround;
        superDash.OnSuperDashHitKick += OnSuperDashHitKick;
        superDash.OnSuperDashHitThrough += OnSuperDashHitThrough;
        superDash.OnSuperDashEnd += OnSuperDashEnd;
        superDash.OnSuperDashHitStarThrough += OnSuperDashHitStarThrough;
        superDash.OnSuperDashHitStarKick += OnSuperDashHitStarKick;
    }
    protected virtual void Update() { }
    protected virtual void OnSuperDashStart() { }
    protected virtual void OnSuperDashHitGround() { }
    protected virtual void OnSuperDashHitKick() { }
    protected virtual void OnSuperDashHitThrough() { }
    protected virtual void OnSuperDashEnd() { }
    protected virtual void OnSuperDashHitStarThrough() { }
    protected virtual void OnSuperDashHitStarKick() { }
}
