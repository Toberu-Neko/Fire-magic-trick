using UnityEngine;
using MoreMountains.Feedbacks;
using System.Threading.Tasks;

public class SuperDash : MonoBehaviour
{
    public GameObject Target;
    [SerializeField] private AnimationCurve superDashIncreaseSpeed;
    [SerializeField] private AnimationCurve superDashReduceSpeed;
    [SerializeField] private float superDashMaxSpeed;
    [SerializeField] private float SuperDashTimeNormal;
    [SerializeField] private float SuperDashTimeFall;
    [SerializeField] private float SuperDashCollingTime;
    [Header("Crash")]
    [SerializeField] private SuperDashCollider _superDashCollider;
    public float CrashForce;
    public float CrashForceUp;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player FireDashStart;
    [SerializeField] private MMF_Player FireDashHit;
    [SerializeField] private MMF_Player FireDashEnd;
    [SerializeField] private MMF_Player FireDashEnd_Interrupt;
    [SerializeField] private MMF_Player Feedbacks_SuperDashCooling;
    [Header("Other")]
    [SerializeField] private GameObject Model;
    [SerializeField] private Transform EndCircle;

    //Script
    private SuperDashCameraCheck _superDashCameraCheck;
    private CharacterController _characterController;
    private SuperDashKickDown _superDashKickDown;
    private ControllerInput _input;
    private PlayerAnimator _playerAnimator;
    private PlayerCollider _playerCollider;
    private SuperDashKick _superDashKick;
    private EnergySystem energySystem;
    private PlayerState _playerState;
    private GameObject player;

    //delegate
    public delegate void SuperDashStartHandler();
    public delegate void SuperDashHitGroundHandler();
    public delegate void SuperDashHitKickHandler();
    public delegate void SuperDashHitThroughHandler();
    public delegate void SuperDashEndHandler();
    public delegate void SuperDashHitStarThroyghHandler();
    public delegate void SuperDashHitStarKickHandler();

    public event SuperDashStartHandler OnSuperDashStart;
    public event SuperDashHitGroundHandler OnSuperDashHitGround;
    public event SuperDashHitKickHandler OnSuperDashHitKick;
    public event SuperDashHitThroughHandler OnSuperDashHitThrough;
    public event SuperDashEndHandler OnSuperDashEnd;
    public event SuperDashHitStarThroyghHandler OnSuperDashHitStarThrough;
    public event SuperDashHitStarKickHandler OnSuperDashHitStarKick;

    //variable
    private Vector3 direction;
    private float superDashSpeed = 0;
    private float superDashTimer = 0;
    private float superDashCoolingtimer;
    private float superDashInterruptTimer;
    private bool isCooling;
    private bool isSuperDashThrough;
    private bool isKick;
    private bool TriggerStart;
    private bool isSuperDashStart;
    public bool isSuperDash;

    private void Start()
    {
        _superDashKick = GameManager.Instance.EnergySystem.GetComponent<SuperDashKick>();
        _playerState = GameManager.Instance._playerState;
        _input = GameManager.Instance._input;
        _superDashCameraCheck = GetComponent<SuperDashCameraCheck>();
        _superDashKickDown = GetComponent<SuperDashKickDown>();
        _characterController = _playerState.GetComponent<CharacterController>();
        _playerCollider = GameManager.Instance.Player.GetComponent<PlayerCollider>();
        _playerAnimator = _playerState.GetComponent<PlayerAnimator>();
        energySystem = _playerState.GetComponent<EnergySystem>();
        player = _playerState.gameObject;

        if (Model.activeSelf == false)
        {
            Model.SetActive(true);
        }
    }
    private void Update()
    {
        GetTarget();
        superDashStartCheck();
        superDash();
        superDashHit();
        superDashThrough();
        IsSuperDashModelCheck();
        superDashCoolingTimer();
        InterruptTimer();
    }
    public void SetIsKick(bool value)
    {
        isKick = value;
    }
    public void EnemyDissapear()
    {
        if (Target == null)
        {
            superDashStop();
        }
    }
    public void ToThroughEnemy()
    {
        _playerState.TakeControl();
        isSuperDash = false;
        superDashTimer = 0;
        FireDashHit.PlayFeedbacks();
        superDashToThrough();
        OnSuperDashHitThrough?.Invoke();
    }
    private async void IsSuperDashModelCheck()
    {
        await Task.Delay(300);
        if(isSuperDash)
        {
            if(Model.activeSelf == true)
            {
                Model.SetActive(false);
            }
        }else
        {
            
        }
    }
    private void Initialization()
    {
        isSuperDashThrough = false;
        isSuperDash = false;
        isKick = false;
        _superDashCollider.SetIsSuperDash(false);
        _playerState.TakeControl();
        _superDashCollider.SetEnemyToClose(false);
        SetTriggerStart(false);
        _playerCollider.ClearColliderTarget();
        superDashTimer = 0;
        superDashSpeed = 0;
    }
    private void GetTarget()
    {
        Target = _superDashCameraCheck.Target;
    }
    private void superDashStartCheck()
    {
        if (_input.LB && Target != null)
        {
            if (!isSuperDash && !isSuperDashThrough)
            {
                if(_superDashCollider.EnemyToClose)
                {
                    _superDashCollider.ToCloseCheckAgain();
                }else
                {
                    if(!TriggerStart)
                    {
                        if(!isCooling)
                        {
                            EnergyCheck();
                        }
                    }
                }
            }
        }
    }
    private void EnergyCheck()
    {
        if(energySystem.canUseEnegy(EnergySystem.SkillType.SuperDash))
        {
            SuperDashColling();
            superDashStart();
            OnSuperDashStart?.Invoke();
        }
    }
    private void superDashCoolingTimer()
    {
        if(isCooling)
        {
            superDashCoolingtimer -= Time.deltaTime;
        }

        if(superDashCoolingtimer <0)
        {
            Feedbacks_SuperDashCooling.StopFeedbacks();
            SetIsCooling(false);
        }
    }
    private void SuperDashColling()
    {
        SetIsCooling(true);
        superDashCoolingtimer = SuperDashCollingTime;
        Feedbacks_SuperDashCooling.PlayFeedbacks();
    }
    public void DecreaseSuperDashTimer(float time)
    {
        superDashCoolingtimer -= time;
    }
    private void superDashStart()
    {
        isSuperDash = true;
        _superDashCollider.SetIsSuperDash(true);
        FireDashStart.PlayFeedbacks();
        superDashInterruptStart();

        SetTriggerStart(true);
        _superDashKickDown.GetTarget(Target);
        _playerAnimator.SuperDashStart();
        _playerState.SetGravityToFire();
    }
    private void superDash()
    {   
        if (isSuperDash)
        {
            if(Target!=null)
            {
                LookAtTarget();
                _playerState.OutControl();
                speedIncrease();
                calaulateDirection();
                move();
            }
            else
            {
                Debug.Log("EnemyDissapear");
                EnemyDissapear();
            }
        }
    }
    private void LookAtTarget()
    {
        _playerState.transform.LookAt(Target.transform);
    }
    private void speedIncrease()
    {
        superDashTimer = speedTimer(superDashTimer, SuperDashTimeNormal);
        superDashSpeed = superDashIncreaseSpeed.Evaluate(superDashTimer) * superDashMaxSpeed;
    }
    private void calaulateDirection()
    {
        direction = calculateDirection(player.transform.position, Target.transform.position).normalized;
    }
    private void move()
    {
        _characterController.Move(direction * superDashSpeed * Time.deltaTime);
    }
    
    private Vector3 calculateDirection(Vector3 start, Vector3 end)
    {
        return end - start;
    }
    
    private float speedTimer(float timer,float dashtime)
    {
        if (timer <= 1f)
        {
            timer += Time.deltaTime/ dashtime;
        }else
        {
            timer = 1;
        }
        return timer;
    }
    private void superDashHit()
    {
        if(isSuperDash && _playerCollider.hit !=null)
        {
            if (_playerCollider.hit.collider.tag == "Enemy")
            {
                _playerState.TakeControl();
                isSuperDash = false;
                superDashTimer = 0;
                FireDashHit.PlayFeedbacks();

                if (_superDashKick.timerCheck(isKick))
                {
                    HitToKickDown();
                    OnSuperDashHitKick?.Invoke();
                }
                else if (_playerState.nearGround)
                {
                    HitGroundEnemy();
                    OnSuperDashHitGround?.Invoke();
                }
                else
                {
                    superDashToThrough();
                    OnSuperDashHitThrough?.Invoke();
                }
            }
            else
            if (_playerCollider.hit.collider.CompareTag("FirePoint"))
            {
                FirePoint point = _playerCollider.hit.collider.GetComponent<FirePoint>();
                point.ToUseFirePoint();

                _playerState.TakeControl();
                isSuperDash = false;
                superDashTimer = 0;
                FireDashHit.PlayFeedbacks();

                if (_superDashKick.timerCheck(isKick))
                {
                    superDashStop();
                    OnSuperDashHitStarKick?.Invoke();
                }
                else if (_playerState.nearGround)
                {
                    superDashStop();
                }
                else
                {
                    superDashToThrough(point);
                    OnSuperDashHitStarThrough?.Invoke();
                }
            }
        }
    }
    private void HitGroundEnemy()
    {
        superDashStop();
    }
    private void HitToKickDown()
    {
        superDashStop();
        _superDashKickDown.KickDown();
    }
    private void superDashStop()
    {
        if(isSuperDash)
        {
        }else
        {
            _superDashCollider.SetIsSuperDash(false);
            _playerState.SetGravityToNormal();
            _playerAnimator.SuperDashEnd();
            FireDashEnd.PlayFeedbacks();
            Instantiate(EndCircle, _playerState.transform.position, Quaternion.identity);
            superDashSpeed = 0;
            Target = null;
            _superDashKickDown.NullTarget();
            SetTriggerStart(false);
            StopInterruptTimer();
            Initialization();
        }
    }
    
    private void superDashToThrough()
    {
        if(_playerCollider.hit != null)
        {
            EnemyHealthSystem enemyHealthSystem = _playerCollider.hit.collider.GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem != null)
            {
                enemyHealthSystem.EnemyDeathRightNow();
            }
            //_playerCollider.hit.collider.gameObject.SetActive(false);
        }
        isSuperDashThrough = true;
        superDashSpeed = superDashMaxSpeed;
    }
    private void superDashToThrough(FirePoint point)
    {
        //point.ToUseFirePoint();
        isSuperDashThrough = true;
        superDashSpeed = superDashMaxSpeed;
    }
    private void superDashThrough()
    {
        if (isSuperDashThrough)
        {
            speedReduce();
            move();
        }
        if(_playerState.isGround && isSuperDashThrough)
        {
            isSuperDashThrough = false;
            superDashStop();
        }
    }
    private void speedReduce()
    {
        superDashTimer = speedTimer(superDashTimer, SuperDashTimeFall);
        superDashSpeed = superDashReduceSpeed.Evaluate(superDashTimer) * superDashMaxSpeed;
    }
    private void superDashInterruptStart()
    {
        SetIsSuperDashStart(true);
    }
    private void InterruptTimer()
    {
        if (isSuperDashStart)
        {
            superDashInterruptTimer += Time.deltaTime;
        }

        if (superDashInterruptTimer > 3f)
        {
            superDashInterruptStop();
            superDashInterruptTimer = 0;
        }
    }
    private void StopInterruptTimer()
    {
        SetIsSuperDashStart(false);
        superDashInterruptTimer = 0;
    }
    private void superDashInterruptStop()
    {
        if (isSuperDash)
        {
            _superDashCollider.SetIsSuperDash(false);
            _playerState.SetGravityToNormal();
            _playerState.ResetVerticalvelocity();
            _playerAnimator.SuperDashEnd();
            FireDashEnd_Interrupt.PlayFeedbacks();
            superDashSpeed = 0;
            Target = null;
            _superDashKickDown.NullTarget();
            SetTriggerStart(false);
            Initialization();
        }
    }

    private void SetTriggerStart(bool active)
    {
        TriggerStart = active;
    }
    private void SetIsCooling(bool value)
    {
        isCooling = value;
    }
    private void SetIsSuperDashStart(bool vaule)
    {
        isSuperDashStart = vaule;
    }
}
