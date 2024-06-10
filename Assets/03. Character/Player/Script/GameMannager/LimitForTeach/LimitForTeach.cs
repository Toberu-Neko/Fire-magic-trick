using UnityEngine;

public class LimitForTeach : MonoBehaviour
{
    public bool useTeach;
    
    [SerializeField] private FireFloat _fireFloat;
    [SerializeField] private FireDash _fireDash;
    [SerializeField] private SuperDash _superDash;
    [SerializeField] private SuperDashCameraCheck _superDashCameraCheck;
    [SerializeField] private SuperDashKick _superDashKick;
    [SerializeField] private SuperDashKickDown _superDashKickDown;

    [SerializeField] private Shooting_Mode _shooting_mode;
    [SerializeField] private Shooting_Magazing _shooting_magazing;

    [SerializeField] private EnergySystem _energySystem;
    [Header("UI")]
    [SerializeField] private GameObject UI_Energy;
    [SerializeField] private GameObject UI_ShootingMagazing;
    [SerializeField] private GameObject UI_ShootingMode;

    private void Start()
    {

        _shooting_mode = GameManager.Instance.ShootingSystem.GetComponent<Shooting_Mode>();

        Initialization();
    }

    public void Initialization()
    {
        if(useTeach)
        {
            SetFloatingScript(false);
            SetBulletMagazingScript(false);
            SetShootingModeScript(false);
            SetDashScript(false);
            SetSuperDashScript(false);
            SetUI_EnergySystem(false);
            SetUI_ShootingMagazing(false);
            SetUI_ShootingMode(false);
        }else
        {
            SetFloatingScript(true);
            SetBulletMagazingScript(true);
            SetShootingModeScript(true);
            SetDashScript(true);
            SetSuperDashScript(true);
            SetUI_EnergySystem(true);
            SetUI_ShootingMagazing(true);
            SetUI_ShootingMode(true);
        }
    }
    #region Set
    public void SetBulletMagazingScript(bool value)
    {
        _shooting_magazing.enabled = value;
    }
    public void SetShootingModeScript(bool value)
    {
        // _shooting_mode.enabled = value;
    }
    public void SetFloatingScript(bool value)
    {
        _fireFloat.enabled = value;
    }
    public void SetDashScript(bool value)
    {
        _fireDash.enabled = value;
    }
    public void SetSuperDashScript(bool value)
    {
        _superDash.enabled = value;
        _superDashCameraCheck.enabled = value;
        _superDashKick.enabled = value;
        _superDashKickDown.enabled = value;
    }
    public void SetUI_EnergySystem(bool value)
    {
        UI_Energy.SetActive(value);
        _energySystem.enabled = value;
    }
    public void SetUI_ShootingMagazing(bool value)
    {
        UI_ShootingMagazing.SetActive(value);
    }
    public void SetUI_ShootingMode(bool value)
    {
        // UI_ShootingMode.SetActive(value);
    }
    #endregion
}
