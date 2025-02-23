using MoreMountains.Feedbacks;
using StarterAssets;
using UnityEngine;
using System.Threading.Tasks;

public class NGP_Basic_Dash : MonoBehaviour
{
    //Script
    protected ControllerInput input;
    protected PlayerState playerState;
    protected NGP_Combo combo;
    protected Move_Our move_Our;
    private EnergySystem energySystem;
    private ThirdPersonController thirdPersonController;
    private CharacterController characterController;
    private FireDashCollider fireDashCollider;
    private Shooting_Check shooting_Check;

    //Feedbacks
    private MMF_Player Feedbacks_Dash;
    private MMF_Player Feedbacks_DashStop;

    //delegate
    public delegate void DashDelegateHandler();
    public event DashDelegateHandler OnDash;

    protected enum DashType
    {
        DashForward,
        DashBackward,
    }
    protected DashType dashType;
    [Header("Setting")]
    protected float collingTime;
    protected float speed;
    protected float dashDistance;
    protected float coolingTimer;

    [Header("variable")]
    [SerializeField] private bool isCooling;
    private bool isDash;
    private bool isButton;
    protected float dashTime;

    protected virtual void Start()
    {
        //Script
        input = GameManager.Instance._input;
        characterController = input.GetComponent<CharacterController>();
        thirdPersonController = input.GetComponent<ThirdPersonController>();
        fireDashCollider = GameManager.Instance.Collider_List.DashCrash.GetComponent<FireDashCollider>();
        playerState = GameManager.Instance.Player.GetComponent<PlayerState>();
        move_Our =GameManager.Instance.Player.GetComponent<Move_Our>();
        combo = GameManager.Instance.NewGamePlay.GetComponent<NGP_Combo>();
        shooting_Check = GameManager.Instance.ShootingSystem.GetComponent<Shooting_Check>();
        energySystem = GameManager.Instance.Player.GetComponent<EnergySystem>();

        //Feedbacks
        Feedbacks_Dash = GameManager.Instance.Feedbacks_List.Dash;
        Feedbacks_DashStop = GameManager.Instance.Feedbacks_List.DashStop;


    }
    protected virtual void Update()
    {
        button();
        system();
    }
    public bool IsDash()
    {
        return isDash;
    }
    private void button()
    {
        if (input.ButtonB && !isButton)
        {
            SetIsButton(true);

            if (!isCooling && !isDash)
            {
                if(energySystem.canUseEnegy(EnergySystem.SkillType.Dash))
                {
                    if (WindButton()) SetIsDashType(DashType.DashBackward);
                    if (FireButton()) SetIsDashType(DashType.DashForward);
                    if (CanCombo()) DashComboSetting();

                    SetIsDash(true);
                    ToDash();
                }
            }
        }
        else
        {
            if (!input.ButtonB && isButton)
            {
                SetIsButton(false);
            }
        }
    }
    protected virtual bool WindButton() { return input.LeftStick == new Vector2(0, 0); }
    protected virtual bool FireButton() { return input.LeftStick != new Vector2(0, 0); }
    protected virtual bool CanCombo() { return false; }
    protected virtual void DashForwardSetting() { }
    protected virtual void DashBackwardSetting() { }
    protected virtual void DashComboSetting() { }
    private void Dash(DashType dashType)
    {
        Vector3 direction = Vector3.zero;
        Vector3 dir = Vector3.zero;

        if (dashType == DashType.DashForward)
        {
            direction = Vector3.forward;
            dir = Quaternion.Euler(0, thirdPersonController.PlayerRotation, 0) * direction;
        }
        if (dashType == DashType.DashBackward)
        {
            direction = Camera.main.transform.forward;
            
            if(input.LeftStick != Vector2.zero)
            {
                //dir = new Vector3(input.LeftStick.x, 0, input.LeftStick.y).normalized;

                direction = Vector3.forward;
                dir = Quaternion.Euler(0, thirdPersonController.PlayerRotation, 0) * direction;
            }
            else
            {
                dir =  new Vector3(-direction.x,0, -direction.z);
            }
            playerState.TurnToNewDirection(shooting_Check.debugTransform.transform.position);
        }
        characterController.Move(dir * speed * Time.deltaTime);
    }
    protected virtual void DashStop() { }
    private void system()
    {
        if (isDash)
        {
            if (dashType == DashType.DashForward) Dash(DashType.DashForward);
            if (dashType == DashType.DashBackward) Dash(DashType.DashBackward);
        }

        if (isCooling)
        {
            coolingTimer -= Time.deltaTime;
        }

        if (coolingTimer <= 0)
        {
            SetIsCooling(false);
            coolingTimer = collingTime;
        }
    }
    public void CoolingStopRightNow()
    {
        SetIsCooling(false);
        coolingTimer = collingTime;
    }
    private async void ToDash()
    {
        CaculateDashTime();
        SetIsDash(true);
        SetIsCooling(true);
        playerState.SetUseMove(false);
        move_Our.ToRun();
        OnDash?.Invoke();
        await Task.Delay((int)(dashTime * 1000));
        SetIsDash(false);
        playerState.SetUseMove(true);
    }
    private void CaculateDashTime()
    {
        dashTime = dashDistance / speed;
    }
    public void DecreaseDashCooling(float value)
    {
        coolingTimer -= value;
    }
    private void OpenCrash()
    {
        fireDashCollider.SetIsDash(true);
    }
    private void CloseCrash()
    {
        fireDashCollider.SetIsDash(false);
        fireDashCollider.SetIsTriggerDamage(false);
    }
    private void SetIsDash(bool value)
    {
        isDash = value;

        if (isDash)
        {
            Feedbacks_Dash.PlayFeedbacks();
            OpenCrash();
        }
        else
        {

            Feedbacks_DashStop.PlayFeedbacks();
            DashStop();
            CloseCrash();
        }
    }
    private void SetIsCooling(bool value)
    {
        isCooling = value;
    }
    private void SetIsDashType(DashType dashType)
    {
        this.dashType = dashType;

        if (dashType == DashType.DashForward) DashForwardSetting();
        if (dashType == DashType.DashBackward) DashBackwardSetting();
    }
    private void SetIsButton(bool value)
    {
        isButton = value;
    }
}
