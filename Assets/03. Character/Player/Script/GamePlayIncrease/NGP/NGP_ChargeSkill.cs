using UnityEngine;
using System.Collections;

public class NGP_ChargeSkill : NGP_Basic_ChargeSkill
{
    [Header("Wind Skill")]
    [SerializeField] private Transform spawn;
    [SerializeField] private Transform circleCard;
    [SerializeField] private Transform circleCardFloat;
    [SerializeField] private Transform circleCardBoom;

    [Header("Fire Skill")]
    [SerializeField] private Transform beacom;
    [Header("OverBurning Release")]
    [SerializeField] private float OverburningRelease=10;
    private GameObject beacomTarget;

    private EnergySystem energySystem;
    protected override void Start()
    {
        base.Start();

        energySystem = GameManager.singleton.Player.GetComponent<EnergySystem>();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override int GetChargeCount()
    {
        return base.GetChargeCount();
    }
    protected override void FireSkillStart()
    {
        base.FireSkillStart();
        beacomTarget = shot.Shot_Gameobj(beacom,0);
    }
    protected override void ChargeSkillFire(int power)
    {
        if(beacomTarget != null)
        {
            Beacon beacon = beacomTarget.GetComponent<Beacon>();
            beacon.StopRightNow();
            skillPower.UseFire();
            
            if(energySystem.isOverBurning)
            {
                energySystem.UseEnergy(OverburningRelease);
            }
        }
    }
    protected override void ChargeSkillWind(int power)
    {
        if (energySystem.isOverBurning)
        {
            energySystem.UseEnergy(OverburningRelease);
        }

        switch (power)
        {
            case 0:
                StartCoroutine(SpawnObjectWithDelay(circleCard,0));
                break;
            case 1:
                StartCoroutine(SpawnObjectWithDelay(circleCardFloat, 0));
                StartCoroutine(SpawnObjectWithDelay(circleCardFloat, 0.25f));
                break;
            case 2:
                StartCoroutine(SpawnObjectWithDelay(circleCardBoom, 0));
                StartCoroutine(SpawnObjectWithDelay(circleCardBoom, 0.25f));
                StartCoroutine(SpawnObjectWithDelay(circleCardBoom, 0.50f));
                break;
        }
    }
    IEnumerator SpawnObjectWithDelay(Transform objectToSpawn,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Instantiate(objectToSpawn, spawn.position, Quaternion.identity);
    }
}
