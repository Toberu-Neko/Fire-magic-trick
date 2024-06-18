using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }
    [field: SerializeField] public PlayerInputHandler InputHandler { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Core Core { get; private set; }
    public Movement Movement { get; private set; }

    [Header("Camera Settings")]
    [SerializeField] protected Transform playerCamera;
    [SerializeField] private bool lockCameraPosition = false;
    [SerializeField] private bool useCameraRotate = true;
    [SerializeField] private float sensitivity_x = 1f;
    [SerializeField] private float sensitivity_y = 0.5f;
    [SerializeField] private float topClamp = 70.0f;
    [SerializeField] private float bottomClamp = -30.0f;
    [SerializeField] private float cameraAngleOverride = 0.0f;
    [SerializeField] private GameObject cinemachineCameraTarget;
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private Vector2 cameraWorkspaceV2;
    public Vector2 CameraPosRelateToPlayer { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkingState WalkState { get; private set; }
    public PlayerRunningState RunningState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    
    public PlayerDashState DashState { get; private set; }

    private void Awake()
    {
        CameraPosRelateToPlayer = new();
        cameraWorkspaceV2 = new();
        cameraWorkspaceV2.Set(playerCamera.position.x - transform.position.x, playerCamera.position.z - transform.position.z);

        Movement = Core.GetCoreComponent<Movement>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, Data, "idle");
        WalkState = new PlayerWalkingState(this, StateMachine, Data, "move");
        RunningState = new PlayerRunningState(this, StateMachine, Data, "move");
        InAirState = new PlayerInAirState(this, StateMachine, Data, "inAir");
        JumpState = new PlayerJumpState(this, StateMachine, Data, "inAir");

        DashState = new PlayerDashState(this, StateMachine, Data, "dash");
    }

    private void Start()
    {
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();

        StateMachine.CurrentState.LogicUpdate();

        cameraWorkspaceV2.Set(playerCamera.position.x - transform.position.x, playerCamera.position.z - transform.position.z);
        CameraPosRelateToPlayer = cameraWorkspaceV2.normalized;
    }

    private void FixedUpdate()
    {
        Core.PhysicsUpdate();

        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();

        if (useCameraRotate)
        {
            CameraRotation();
        }
    }

    private void AnimationActionTrigger() => StateMachine.CurrentState.AnimationActionTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void AnimationStartMovementTrigger() => StateMachine.CurrentState.AnimationStartMovementTrigger();

    private void AnimationStopMovementTrigger() => StateMachine.CurrentState.AnimationStopMovementTrigger();

    private void AnimationSFXTrigger() => StateMachine.CurrentState.AnimationSFXTrigger();


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
}
