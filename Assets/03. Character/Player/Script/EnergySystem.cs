using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public enum SkillType
    {
        Reload,
        Dash,
        SuperDash,
        Kick,
        Float
    }
    //Script
    private EnergySystemUI energySystemUI;
    private DeathSystem deathSystem;
    private PlayerState playerState;
    //Variable
    [Header("Energy")]
    public bool isOverBurning;
    public float Energy;
    public float Energy_overBurning;
    [SerializeField] private float StartEnergy;

    [Header("Recover")]
    [SerializeField] private float recoverRange;
    [SerializeField] private float recoverTime;
    [SerializeField] private float recover;

    [Header("OverBurning")]
    [SerializeField] private AnimationCurve overBurningCurve;
    [SerializeField] private float MaxParticleSize;
    [SerializeField] private AnimationCurve overBurningNumberCurve;
    [SerializeField] private float MaxParticleEmission;
    [SerializeField] private AnimationCurve overBurningScreen;
    [SerializeField] private float MaxParticleSize_Screen;
    [SerializeField] private AnimationCurve overBurningNumberScreen;
    [SerializeField] private float MaxParticleEmission_Screen;
    [Space(10)]
    [SerializeField] private float recoverRange_overBurning;
    [SerializeField] private float recoverTime_overBurning;
    [SerializeField] private float recover_overBurning;
    [Space(10)]

    [Header("GetEnergy")]
    private float timer;
    private float timer_overBurning;
    private bool isRecover;
    private bool isRecover_overBurning;

    [Header("Cost")]
    [SerializeField] private bool isTestMode;
    [SerializeField] private float SuperDashCost = 10;
    [SerializeField] private float ReloadCost = 10;
    [SerializeField] private float FloatCost = 10;
    [SerializeField] private float DashCost = 10;
    [SerializeField] private float KickCost = 10;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player Feedbacks_NoEnegy;
    [SerializeField] private MMF_Player Feedback_GetEnergy;
    [SerializeField] private MMF_Player Feedback_PlayerDeath;
    [Header("VFX")]
    [SerializeField] private ParticleSystem VFX_overBurning;
    [SerializeField] private ParticleSystem VFX_overBurning_Screen;

    private void Start()
    {
        energySystemUI = GameManager.Instance.UISystem.GetComponent<EnergySystemUI>();
        deathSystem = GameManager.Instance.UISystem.GetComponent<DeathSystem>();
        playerState = GameManager.Instance.Player.GetComponent<PlayerState>();
    }
    private void Update()
    {
        RecoverSystem();
        overBurningSystem();
    }
    public bool canUseEnegy(SkillType type)
    {
        float need =0;
        switch (type)
        {
            case SkillType.Reload:
                need = ReloadCost;
                break;
            case SkillType.Dash:
                need = DashCost;
                break;
            case SkillType.SuperDash:
                need = SuperDashCost;
                break;
            case SkillType.Kick:
                need = KickCost;
                break;
            case SkillType.Float:
                need = FloatCost;
                break;
        } //Check type

        if(Energy > need) //cost
        {
            UseEnergy(need);
            return true;
        }else
        {
            return false;
        }
    }
    public void UseEnergy(float value)
    {
        if(isOverBurning)
        {
            if(leaveOverBurning(value))
            {
                float allEnergy = Energy + Energy_overBurning;
                float leaveEnergy =100 - allEnergy - value;

                Decrease(leaveEnergy);
                leaveOverBurning();
            }
            Decrease_overburning(value);
        }else
        {
            Decrease(value);
        }
    }
    private bool leaveOverBurning(float value)
    {
        float allEnergy = Energy + Energy_overBurning;
        float newEnergy = allEnergy - value;

        if(newEnergy <100)
        {
            return true;
        }else
        {
            return false;
        }
    }
    public void GetEnergyByDamage(float value)
    {
        if(!isOverBurning)
        {
            if(willOverburning(value))
            {
                float overEnergy = Energy + value - 100;
                Increase_overburning(overEnergy);
                ToOverBurning();
            }
            Increase(value);
        }else
        {
            Increase_overburning(value);
        }

        if(Energy_overBurning >=100)
        {
            playerDeath();
        }
    }
    public async void playerDeath()
    {
        Feedback_PlayerDeath.PlayFeedbacks();
        Decrease_overburning(100);
        Decrease(100);
        setIsOverBurning(false);
        playerState.SetUseCameraRotate(false);
        playerState.OutControl();
        await Task.Delay(1500);
        deathSystem.Death();
    }

    private bool willOverburning(float value)
    {
        float newEnergy = Energy + value;

        if(newEnergy > 100)
        {
            return true;
        }else
        {
            return false;
        }
    }
    private void overBurningSystem()
    {
        var mainMode = VFX_overBurning.main;
        mainMode.startSize = overBurningCurve.Evaluate(Energy_overBurning/100) * MaxParticleSize;

        var emission = VFX_overBurning.emission;
        emission.rateOverTime = overBurningNumberCurve.Evaluate(Energy_overBurning / 100) * MaxParticleEmission;

        var mainMove_Screen = VFX_overBurning_Screen.main;
        mainMove_Screen.startSize = overBurningNumberScreen.Evaluate(Energy_overBurning / 100) * MaxParticleSize_Screen;

        var emission_Screen = VFX_overBurning_Screen.emission;
        emission_Screen.rateOverTime = overBurningNumberScreen.Evaluate(Energy_overBurning/100)* MaxParticleEmission_Screen;
    }
    private void ToOverBurning()
    {
        setIsOverBurning(true);
    }
    private void leaveOverBurning()
    {
        setIsOverBurning(false);
    }
    public void GetEnergy(float value)
    {
        Feedback_GetEnergy.PlayFeedbacks();
        Increase(value);

        if(isOverBurning)
        {
            Decrease_overburning(value / 4);
            if(Energy_overBurning==0)
            {
                setIsOverBurning(false);
            }
        }
    }
    #region Increase Decrease
    private void Increase(float energy)
    {
        Energy += energy;

        if(Energy>100)
        {
            Energy = 100;
        }
        UpdateUI(Energy);
    }
    private void Decrease(float energy)
    {
        Energy -= energy;

        if (Energy <0)
        {
            Energy = 0;
        }
        UpdateUI(Energy);
    }
    private void UpdateUI(float Value)
    {
        float value = Value / 100;
        if(energySystemUI != null)
        {
            energySystemUI.UpdateBar(value);
        }
    }
    private void Increase_overburning(float energy)
    {
        Energy_overBurning += energy;

        if (Energy_overBurning > 100)
        {
            Energy_overBurning = 100;
        }
        UpdateUI_overBurning(Energy_overBurning);
    }
    private void Decrease_overburning(float energy)
    {
        Energy_overBurning -= energy;

        if (Energy_overBurning < 0)
        {
            Energy_overBurning = 0;
        }
        UpdateUI_overBurning(Energy_overBurning);
    }
    public void UpdateUI_overBurning(float Value)
    {
        float value = Value / 100;

        if (energySystemUI != null)
        {
            energySystemUI.UpdateBarOverBurning(value);
        }
    }
    #endregion
    #region Recover
    private void RecoverSystem()
    {
        RecoverCheck();
        recoverTimer();
        RecoverCheck_overburning();
        recoverTimer_overburning();
    }
    private void RecoverCheck()
    {
        bool condition = Energy < recoverRange;
        isRecover = condition ? true : false;
    }
    private void recoverTimer()
    {
        if (isRecover)
        {
            timer += Time.deltaTime;

            if (timer > recoverTime)
            {
                Increase(recover);
                timer = 0;
            }
        }
    }
    private void RecoverCheck_overburning()
    {
        bool condition = Energy_overBurning < recoverRange_overBurning;
        if(isOverBurning)
        {
            isRecover_overBurning = condition ? true : false;
        }else
        {
            isRecover_overBurning = false;
        }
    }
    private void recoverTimer_overburning()
    {
        if (isRecover_overBurning)
        {
            timer_overBurning += Time.deltaTime;

            if (timer_overBurning > recoverTime_overBurning)
            {
                Increase_overburning(recover_overBurning);
                timer_overBurning = 0;
            }
        }
    }
    #endregion
    private void setIsOverBurning(bool active)
    {
        isOverBurning = active;
    }
}
