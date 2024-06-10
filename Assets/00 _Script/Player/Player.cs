using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }
    [field: SerializeField] public PlayerInputHandler InputHandler { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }

    [Header("Camera Settings")]
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

    public PlayerIdleState IdleState { get; private set; }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, Data, "idle");
    }

    private void Start()
    {
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
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
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
