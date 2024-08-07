using Cinemachine;
using MoreMountains.Tools;
using UnityEngine;

[RequireComponent(typeof(CardSystem), typeof(PlayerVFXController), typeof(PlayerToCombat))]
public class Player : MonoBehaviour, IPlayerHandler, IDataPersistance
{
    public PlayerStateMachine StateMachine { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Core Core { get; private set; }
    [field: SerializeField] public CardSystem CardSystem { get; private set; }
    [field: SerializeField] public PlayerInputHandler InputHandler { get; private set; }
    [field: SerializeField] public PlayerVFXController VFXController { get; private set; }
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private CinemachineImpulseSource dashHitImpluseSource;
    [SerializeField] private Transform defaultRespawnPos;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private AudioSource superDashAudio;

    public Movement Movement { get; private set; }
    public Stats Stats { get; private set; }
    private Combat combat;

    [Header("Camera Objects")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject NormalCam;
    [SerializeField] private GameObject RunCam;
    [SerializeField] private GameObject SkillCam;
    [SerializeField] private GameObject AimCam;
    [SerializeField] private GameObject superDashCam;
    [SerializeField] private GameObject DeathCam;
    private ActiveCamera activeCamera;
    private bool controlCamBySpeed;
    public enum ActiveCamera
    {
        None,
        Aim,
        Death,
        Skill,
        SuperDash,
        DeterminBySpeed
    }

    [Header("Camera Settings")]
    [SerializeField] private GameObject cinemachineCameraTarget;
    [SerializeField] private bool lockCameraPosition = false;
    [field: SerializeField] public bool UseCameraRotate { get; set; }
    [SerializeField] private float sensitivity_x = 1f;
    [SerializeField] private float sensitivity_y = 0.5f;
    [SerializeField] private float topClamp = 70.0f;
    [SerializeField] private float bottomClamp = -30.0f;
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private Vector2 cameraWorkspaceV2;
    public Vector2 CameraPosRelateToPlayer { get; private set; }
    public Vector3 RespawnPosition { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkingState WalkingState { get; private set; }
    public PlayerRunningState RunningState { get; private set; }
    public PlayerAimIdleState AimIdleState { get; private set; }
    public PlayerAimWalkingState AimWalkingState { get; private set; }

    public PlayerInAirState InAirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    
    public PlayerDashState DashState { get; private set; }
    public PlayerSuperDashState SuperDashState { get; private set; }
    public PlayerAfterSuperDashJump AfterSuperDashJump { get; private set; }
    public PlayerFireballState FireballState { get; private set; }
    public PlayerSuperJumpState SuperJumpState { get; private set; }

    public PlayerWindAltState WindAltState { get; private set; }
    public PlayerFireAltState FireAltState { get; private set; }

    public PlayerDeathState DeathState { get; private set; }
    public PlayerRespawnState RespawnState { get; private set; }
    public PlayerCantControlState CantControlState { get; private set; }
    public PlayerLoadingState LoadingState { get; private set; }
    private bool firstTimePlaying;
    private bool loadFinished;

    private void Awake()
    {
        CameraPosRelateToPlayer = new();
        cameraWorkspaceV2 = new();
        cameraWorkspaceV2.Set(playerCamera.position.x - transform.position.x, playerCamera.position.z - transform.position.z);
        UseCameraRotate = true;
        loadFinished = false;

        Movement = Core.GetCoreComponent<Movement>();
        Stats = Core.GetCoreComponent<Stats>();
        combat = Core.GetCoreComponent<Combat>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, Data, "idle");
        WalkingState = new PlayerWalkingState(this, StateMachine, Data, "move");
        RunningState = new PlayerRunningState(this, StateMachine, Data, "move");

        AimIdleState = new PlayerAimIdleState(this, StateMachine, Data, "move");
        AimWalkingState = new PlayerAimWalkingState(this, StateMachine, Data, "move");

        InAirState = new PlayerInAirState(this, StateMachine, Data, "inAir");

        JumpState = new PlayerJumpState(this, StateMachine, Data, "jump");
        DashState = new PlayerDashState(this, StateMachine, Data, "dash");
        SuperDashState = new PlayerSuperDashState(this, StateMachine, Data, "inAir");
        AfterSuperDashJump = new PlayerAfterSuperDashJump(this, StateMachine, Data, "backFlip");
        FireballState = new PlayerFireballState(this, StateMachine, Data, "inAir");
        SuperJumpState = new PlayerSuperJumpState(this, StateMachine, Data, "inAir");

        WindAltState = new PlayerWindAltState(this, StateMachine, Data, "inAir");
        FireAltState = new PlayerFireAltState(this, StateMachine, Data, "inAir");

        DeathState = new PlayerDeathState(this, StateMachine, Data, "death");
        RespawnState = new PlayerRespawnState(this, StateMachine, Data, "respawn");
        CantControlState = new PlayerCantControlState(this, StateMachine, Data, "idle");
        LoadingState = new PlayerLoadingState(this, StateMachine, Data, "idle");
    }

    private void OnEnable()
    {
        Stats.Health.OnCurrentValueZero += HandleHealthZero;
        Stats.OnBurnChanged += HandleBurnChanged;

        combat.OnDamaged += HandleOnDamaged;
    }

    private void OnDisable()
    {
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
        Stats.OnBurnChanged -= HandleBurnChanged;

        combat.OnDamaged -= HandleOnDamaged;
    }

    private void Start()
    {
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        StateMachine.Initialize(LoadingState);
        Stats.Health.Init();

        LoadSceneManager.Instance.OnAdditiveSceneAlreadyLoaded += HandleFinishLoading;
        LoadSceneManager.Instance.OnLoadingAdditiveProgress += HandleAdditiveLoading;
    }

    private void OnDestroy()
    {
        LoadSceneManager.Instance.OnAdditiveSceneAlreadyLoaded -= HandleFinishLoading;
        LoadSceneManager.Instance.OnLoadingAdditiveProgress -= HandleAdditiveLoading;
    }

    private void HandleOnDamaged()
    {
        UIManager.Instance.HudUI.HudVFX.HitPlayerEffect();
    }
    #region HandleLoading

    private void HandleAdditiveLoading(float obj)
    {
        if (obj == 1f)
        {
            HandleFinishLoading();
        }
    }

    private void HandleFinishLoading()
    {
        if (!loadFinished)
        {
            loadFinished = true;
            StateMachine.ChangeState(RespawnState);
        }
    }
    #endregion

    private void Update()
    {
        Core.LogicUpdate(); 
        UIManager.Instance.HudUI.SetBar(Stats.Health.CurrentValuePercentage);

        StateMachine.CurrentState.LogicUpdate();
        Anim.SetFloat("speed", Movement.CurrentVelocityXZMagnitude);

        cameraWorkspaceV2.Set(playerCamera.position.x - transform.position.x, playerCamera.position.z - transform.position.z);
        CameraPosRelateToPlayer = cameraWorkspaceV2.normalized;

        if(Stats.IsInvincible && !Movement.CanSetVelocity)
        {
            Movement.SetCanSetVelocity(true);
        }

        if (Stats.Health.CurrentValue > Stats.Health.InitValue && !Stats.InCombat)
        {
            DecreaseHealthUntilInit(Data.healthDecreaseRate * Time.deltaTime);
        }
        else if (Stats.IsBurning && !Stats.IsInvincible)
        {
            Stats.Health.Decrease(Data.healthDecreaseRateBurning * Time.deltaTime);
        }

        if (Movement.CurrentVelocityXZMagnitude > Data.slowRunSpeed)
        {
            UIManager.Instance.HudUI.HudVFX.RunSpeedLineEffect(true);

            if (controlCamBySpeed)
            {
                SetRunCam();
            }
        }
        else
        {
            UIManager.Instance.HudUI.HudVFX.RunSpeedLineEffect(false);
            if (controlCamBySpeed)
            {
                SetNormalCam();
            }
        }
    }

    private void FixedUpdate()
    {
        Core.PhysicsUpdate();

        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();

        if (UseCameraRotate && !GameManager.Instance.IsPaused)
        {
            CameraRotation();
        }
    }

    #region Animation Triggers
    private void AnimationActionTrigger() => StateMachine.CurrentState.AnimationActionTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void AnimationStartMovementTrigger() => StateMachine.CurrentState.AnimationStartMovementTrigger();

    private void AnimationStopMovementTrigger() => StateMachine.CurrentState.AnimationStopMovementTrigger();

    private void AnimationSFXTrigger() => StateMachine.CurrentState.AnimationSFXTrigger();
    #endregion

    #region Camera
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (InputHandler.RawMouseInput.sqrMagnitude >= _threshold && !lockCameraPosition)
        {
            
            float deltaTimeMultiplier;
            if (InputHandler.ActiveGameDevice == PlayerInputHandler.GameDevice.Keyboard)
            {
                deltaTimeMultiplier = 1f;
            }
            else
            {
                deltaTimeMultiplier = Time.deltaTime;
            }

            _cinemachineTargetYaw += InputHandler.RawMouseInput.x * deltaTimeMultiplier * sensitivity_y;
            _cinemachineTargetPitch += InputHandler.RawMouseInput.y * deltaTimeMultiplier * sensitivity_y;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void ChangeActiveCam(ActiveCamera newActiveCam)
    {
        if (activeCamera == newActiveCam)
        {
            return;
        }

        activeCamera = newActiveCam;

        switch (activeCamera)
        {
            case ActiveCamera.Aim:
                controlCamBySpeed = false;
                NormalCam.SetActive(false);
                RunCam.SetActive(false);
                SkillCam.SetActive(false);
                AimCam.SetActive(true);
                DeathCam.SetActive(false);
                superDashCam.SetActive(false);
                break;
            case ActiveCamera.Skill:
                controlCamBySpeed = false;
                NormalCam.SetActive(false);
                RunCam.SetActive(false);
                SkillCam.SetActive(true);
                AimCam.SetActive(false);
                DeathCam.SetActive(false);
                superDashCam.SetActive(false);
                break;
            case ActiveCamera.Death:
                controlCamBySpeed = false;
                NormalCam.SetActive(false);
                RunCam.SetActive(false);
                SkillCam.SetActive(false);
                AimCam.SetActive(false);
                DeathCam.SetActive(true);
                superDashCam.SetActive(false);
                break;
            case ActiveCamera.SuperDash:
                controlCamBySpeed = false;
                NormalCam.SetActive(false);
                RunCam.SetActive(false);
                SkillCam.SetActive(false);
                AimCam.SetActive(false);
                DeathCam.SetActive(false);
                superDashCam.SetActive(true);
                break;
            case ActiveCamera.DeterminBySpeed:
                controlCamBySpeed = true;
                SkillCam.SetActive(false);
                AimCam.SetActive(false);
                DeathCam.SetActive(false);
                superDashCam.SetActive(false);
                break;
            
        }
    }

    private void SetRunCam()
    {
        if(RunCam.activeInHierarchy)
        {
            return;
        }
        NormalCam.SetActive(false);
        RunCam.SetActive(true);
        SkillCam.SetActive(false);
        AimCam.SetActive(false);
        DeathCam.SetActive(false);
    }

    private void SetNormalCam()
    {
        if (NormalCam.activeInHierarchy)
        {
            return;
        }
        NormalCam.SetActive(true);
        RunCam.SetActive(false);
        SkillCam.SetActive(false);
        AimCam.SetActive(false);
        DeathCam.SetActive(false);
    }
    #endregion

    private void HandleHealthZero()
    {
        if (!DeathState.CheckInState())
        {
            StateMachine.ChangeState(DeathState);
        }
    }

    private void HandleBurnChanged(bool obj)
    {
        if(obj)
        {
            VFXController.SetBurningVFX((1f - Stats.Health.CurrentValuePercentage) / 0.5f);
        }
        else
        {
            VFXController.SetBurningVFX(0f);
        }
        UIManager.Instance.HudUI.HudVFX.OverburnEffect(obj);
    }

    public void TeleportToSavepoint()
    {
        Teleport(RespawnPosition);
    }

    public void SetColliderAndModel(bool value)
    {
        SetCollider(value);
        SetModel(value);
        Stats.SetInvincible(!value);
    }

    public void DecreaseHealthUntilInit(float value)
    {
        Stats.Health.DecreaseUntilInitValue(value);
    }

    public void SetSuperDashAudio(bool value)
    {
        if (value)
        {
           superDashAudio.Play();
        }
        else
        {
            superDashAudio.Stop();
        }
    }

    #region IPlayerHandler

    public void SetCollider(bool value)
    {
        col.isTrigger = !value;
    }

    public void SetModel(bool value)
    {
        playerModel.SetActive(value);
        VFXController.SetModelVFX(value);
    }

    public void GotoCantControlState()
    {
        StateMachine.ChangeState(CantControlState);
    }

    public void FinishCantControlState()
    {
        CantControlState.SetIsAbilityDone();
    }

    public void GotoAfterSuperDashJumpState()
    {
        StateMachine.ChangeState(AfterSuperDashJump);
    }

    public void Teleport(Vector3 position)
    {
        if(position == Vector3.zero)
        {
            position = defaultRespawnPos.position;
        }
        firstTimePlaying = false;
        transform.position = position;
    }

    public void SetRespawnPosition(Vector3 position)
    {
        RespawnPosition = position;
    }
    #endregion

    public void DoDashHitImpluse(float value)
    {
        dashHitImpluseSource.GenerateImpulse(value);
    }

    #region IDataPersistance
    public void LoadData(GameData data)
    {
        if (data.playerRespawnPosition == Vector3.zero)
        {
            RespawnPosition = defaultRespawnPos.position;
            Teleport(RespawnPosition);
        }
        else
        {
            RespawnPosition = data.playerRespawnPosition;
            firstTimePlaying = data.firstTimePlaying;
            Teleport(RespawnPosition);
        }
    }

    public void SaveData(GameData data)
    {
        data.playerRespawnPosition = RespawnPosition;
        data.firstTimePlaying = firstTimePlaying;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if (Data)
        {
            Gizmos.DrawWireSphere(transform.position, Data.closeRangeDetectRadius);
            Gizmos.DrawWireSphere(transform.position, Data.midRangeDetectRadius);
            Gizmos.DrawWireSphere(transform.position, Data.longRangeDetectRadius);
            Gizmos.DrawWireSphere(transform.position, Data.zeroRangeDetectRadius);
        }
    }
}
