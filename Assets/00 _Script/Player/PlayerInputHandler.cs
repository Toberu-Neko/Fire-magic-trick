using MagicaCloth2;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region GameDevice
    public enum GameDevice
    {
        Keyboard,
        Gamepad
    }

    public GameDevice ActiveGameDevice { get; private set; }
    private void HandleActionChange(object arg1, InputActionChange inputActionChange)
    {
        if (inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction)
        {
            InputAction inputAction = (InputAction)arg1;

            if (inputAction.activeControl.device.displayName == "VirtualMouse")
            {
                return;
            }

            if (inputAction.activeControl.device is Gamepad)
            {
                if (ActiveGameDevice != GameDevice.Gamepad)
                {
                    ChangeActiveGameDevice(GameDevice.Gamepad);
                }
            }
            else if ((inputAction.activeControl.device is Keyboard && inputAction.activeControl.device is not Gamepad) || inputAction.activeControl.device is Mouse)
            {
                if (ActiveGameDevice != GameDevice.Keyboard)
                {
                    ChangeActiveGameDevice(GameDevice.Keyboard);
                }
            }
        }
    }
    private void ChangeActiveGameDevice(GameDevice newGameDevice)
    {
        ActiveGameDevice = newGameDevice;
    }

    #endregion


    public static PlayerInputHandler Instance { get; private set; }
    private GameManager gameManager;
    private PlayerInput playerInput;
    public Camera MainCam { get; private set; }

    public Vector2 RawMouseInput { get; private set; }
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool SprintInput { get; private set; }
    public bool FireTransferInput { get; private set; }


    public bool AimInput { get; private set; }
    public bool HoldAimInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool HoldAttackInput { get; private set; }
    public bool BigJumpInput { get; private set; }
    public bool SkillInput { get; private set; }
    public bool SkillHoldInput { get; private set; }

    public bool DebugInput { get; private set; }
    public bool TurnOffUI { get; private set; }

    public bool ESCInput { get; private set; }
    public bool InteractInput { get; private set; }


    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InputSystem.onActionChange += HandleActionChange;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        MainCam = Camera.main;
        gameManager = GameManager.Instance;
    }
    private void OnDisable()
    {
        InputSystem.onActionChange -= HandleActionChange;
    }

    public void ResetAllInput()
    {
        RawMovementInput = Vector2.zero;
        NormInputX = 0;
        NormInputY = 0;

        JumpInput = false;
        DashInput = false;
        DashInputStop = false;
        AttackInput = false;
        BigJumpInput = false;
        SkillInput = false;
        SkillHoldInput = false;
        DebugInput = false;
        TurnOffUI = false;
        ESCInput = false;
        InteractInput = false;

        AimInput = false;
        HoldAimInput = false;
        HoldAttackInput = false;
        SprintInput = false;
        FireTransferInput = false;
    }

    public void OnESCInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ESCInput = true;
        }
        if (context.canceled)
        {
            ESCInput = false;
        }
    }

    public void UseESCInput() => ESCInput = false;

    public void OnTurnOffUIInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TurnOffUI = true;
        }
        if (context.canceled)
        {
            TurnOffUI = false;
        }
    }

    public void UseTurnOffUIInput()
    {
        TurnOffUI = false;
    }

    public void OnDebugInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            DebugInput = true;
        }
        if (context.canceled)
        {
            DebugInput = false;
        }
    }

    public void UseDebugInput()
    {
        DebugInput = false;
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            InteractInput = true;
        }
        if (context.canceled)
        {
            InteractInput = false;
        }
    }

    public void UseInteractInput()
    {
        InteractInput = false;
    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            // Debug.Log("AttackInput");
            AttackInput = true;
            HoldAttackInput = true;
        }
        if (context.canceled)
        {
            AttackInput = false;
            HoldAttackInput = false;
        }
    }

    public void UseAttackInput() => AttackInput = false;

    public void OnAimInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            AimInput = true;
            HoldAimInput = true;
        }
        if (context.canceled)
        {
            AimInput = false;
            HoldAimInput = false;
        }
    }

    public void UseAimInput() => AimInput = false;

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = UnityEngine.Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = UnityEngine.Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        RawMouseInput = context.ReadValue<Vector2>();
    }

    public void OnSkillInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            SkillInput = true;
            SkillHoldInput = true;
        }
        if (context.canceled)
        {
            SkillInput = false;
            SkillHoldInput = false;
        }
    }

    public void UseSkillInput() => SkillInput = false;

    public void OnBigJumpInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            BigJumpInput = true;
        }
        if (context.canceled)
        {
            BigJumpInput = false;
        }
    }

    public void UseBigJumpInput() => BigJumpInput = false;

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void UseDashInput() => DashInput = false;

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }

    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            SprintInput = true;
        }
        if (context.canceled)
        {
            SprintInput = false;
        }
    }

    public void UseSprintInput() => SprintInput = false;

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }
    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnFireTransfer(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            FireTransferInput = true;
        }
        if (context.canceled)
        {
            FireTransferInput = false;
        }
    }

    public void UseFireTransferInput() => FireTransferInput = false;
}
