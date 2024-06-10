using UnityEngine;

public class NGP_SuperJump : NGP_Basic_SuperJump
{
    [SerializeField] private float overBurning_UseEnergy=10;
    [SerializeField] private float heavyPressureGravity = 1f;
    [SerializeField] private Transform VFX_SuperDashJump_Wind;
    [SerializeField] private Transform VFX_SuperDashJump_Fire;
    //Script
    private EnergySystem energySystem;
    //VFX
    private ParticleSystem VFX_FireCircle;
    private ParticleSystem VFX_WindCricle;

    //variable
    private bool isFire;
    private bool isWind;
    protected override void Start()
    {
        base.Start();

        //vfx
        VFX_FireCircle = GameManager.Instance.VFX_List.VFX_FireCircle;
        VFX_WindCricle = GameManager.Instance.VFX_List.VFX_WindCricle;

        energySystem = GameManager.Instance.Player.GetComponent<EnergySystem>();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override bool canUseSuperJump()
    {
        if(skillPower.FirePower >= 3 || skillPower.WindPower >= 3)
        {
            return true;
        }else
        {
            return false;
        }
    }
    protected override bool isFireJump()
    {
        if ( skillPower.FirePower >= 3)
        {
            return true;
        } else
        {
            return false;
        }
    }
    protected override bool isWindJump()
    {
        if (skillPower.WindPower>= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected override void SuperJump_wind()
    {
        if(energySystem.isOverBurning)
        {
            energySystem.UseEnergy(overBurning_UseEnergy);
        }
        state.SetGravityToNormal();
        jump.SuperJump(SuperJumpHeight);
        Instantiate(VFX_SuperDashJump_Wind, transform.position, Quaternion.identity);
        isWind = true;
    }
    protected override void SuperJump_fire()
    {
        if (energySystem.isOverBurning)
        {
            energySystem.UseEnergy(overBurning_UseEnergy);
        }
        state.SetGravityToNormal();
        jump.SuperJump(SuperJumpHeight);
        Instantiate(VFX_SuperDashJump_Fire, transform.position, Quaternion.identity);
        isFire = true;
    }
    protected override void heavy()
    {
        if(input.ButtonY)
        {
            state.SetGravity(heavyPressureGravity);
        }
    }
    protected override void heavyEnd()
    {
        state.SetGravityToNormal();
        if(isFire)
        {
            VFX_FireCircle.Play();
            Instantiate(VFX_SuperDashJump_Fire, transform.position, Quaternion.identity);
        }
        if(isWind)
        {
            VFX_WindCricle.Play();
            Instantiate(VFX_SuperDashJump_Wind, transform.position, Quaternion.identity);
        }
    }
    public void CancelHeavy()
    {
        state.SetGravityToNormal();
        if(isHeavy) isHeavy = false;
        if(isHeavyPrepare) isHeavyPrepare = false;
    }
}
