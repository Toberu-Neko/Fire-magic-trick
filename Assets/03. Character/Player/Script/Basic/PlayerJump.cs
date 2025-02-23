using StarterAssets;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float maxPreInputTime = 0.1f;
    [SerializeField] private float JumpTimeout = 0.1f;
    [SerializeField] private float JumpToGroundTime = 0.1f;
    
    //delegate 
    public delegate void JumpEvent();
    public event JumpEvent OnSuperJump;

    //Script
    private ThirdPersonController _thirdPersonController;
    private ControllerInput _input;

    //value
    private float preInputTimer = 0f;
    private bool isPreInput = false;
    private bool trigger = false;
    private bool triggerJumpGround = false;

    private void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _input = GetComponent<ControllerInput>();
    }

    private void Update()
    {
        preInputSystem();
        //JumpToGround();
    }
    #region PreInput
    #region System
    private void preInputSystem()
    {
        preInputButton();
        preInput();
        preInputEventCheck();
    }
    private void preInputButton()
    {
        if (_input.ButtonA && !trigger)
        {
            isPreInput = true;
            trigger = true;
        }
        if(!_input.ButtonA)
        {
            trigger = false;
        }
    }
    private void preInput()
    {
        if(isPreInput)
        {
            preInputTimer += Time.deltaTime;
        }

        if(preInputTimer>maxPreInputTime)
        {
            Initialization();
        }
    }
    private void Initialization()
    {
        preInputTimer = 0f;
        isPreInput = false;
    }
    #endregion
    private void preInputEventCheck()
    {
        if (isPreInput)
        {
            if(_thirdPersonController.Grounded && _thirdPersonController._jumpTimeoutDelta <= 0f)
            {
                if(!_thirdPersonController.isJump)
                {
                    jump();
                }
                Initialization();
            }
        }
    }
    #endregion
    private async void jump()
    {
        await Task.Delay((int)(JumpTimeout *1000f));
        _thirdPersonController.Jump();
    }
    public async void SuperJump(float jumheight)
    {
        await Task.Delay((int)(JumpTimeout * 1000f));
        _thirdPersonController.SetVerticalVelocity(0);
        _thirdPersonController.Jump(jumheight);

        OnSuperJump?.Invoke(); //super jump;
    }
    private void JumpToGround()
    {
        if(_thirdPersonController.Grounded)
        {
            if(!triggerJumpGround)
            {
                _thirdPersonController.useMove = false;
                TriggerJumpGround();
            }
        }else
        {
            triggerJumpGround = false;
        }
    }
    private async void TriggerJumpGround()
    {
        await Task.Delay((int)(JumpToGroundTime * 1000f));
        _thirdPersonController.useMove = true;
        triggerJumpGround = true;
    }

}
