using MoreMountains.Feedbacks;
using Unity.VisualScripting;
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
    private EnergySystemUI _energySystemUI;

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

    [Header("GetEnergy")]
    private float timer;
    private bool isRecover;

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


    private void Start()
    {
        _energySystemUI = GameManager.singleton.UISystem.GetComponent<EnergySystemUI>();
    }
    private void Update()
    {
        RecoverSystem();
        UseEnergyTest();
    }
    private void UseEnergyTest()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Decrease(10);
        }
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
        if(_energySystemUI != null)
        {
            _energySystemUI.UpdateBar(value);
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

        if (_energySystemUI != null)
        {
            _energySystemUI.UpdateBarOverBurning(value);
        }
    }
    #endregion
    #region Recover
    private void RecoverSystem()
    {
        RecoverCheck();
        recoverTimer();
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
    #endregion
    private void setIsOverBurning(bool active)
    {
        isOverBurning = active;
    }
}
