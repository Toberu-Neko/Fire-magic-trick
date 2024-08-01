using MoreMountains.Feedbacks;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.PlayerLoop;

public class EnemyHealthSystem : MonoBehaviour
{
    [SerializeField] private bool isTeachEnemy;
    [SerializeField] private bool isOnceEnemy;
    [SerializeField] private bool isRebirthHide;
    [Header("State")]
    private bool isHurt;
    private bool isSteam;
    private bool isFireVFX;
    private bool isShockVFX;
    public bool Boom;

    [Header("Feedbacks")]
    [SerializeField] private EyeColorController _eye;
    [SerializeField] private MMF_Player feedbacks_Steam;
    [SerializeField] private MMF_Player feedbacks_Fire;
    [SerializeField] private MMF_Player feedbacks_Shock;
    [SerializeField] private MMF_Player feedbacks_Boom;
    [SerializeField] private MMF_Player feedbacks_FlyBoom;
    [SerializeField] private GameObject VFX_Death;

    [Header("KickBack")]
    public float kickBackRatio;
    public bool kickBackGuard = false;

    [Header("Spread Area")]
    [SerializeField] private GameObject spreadArea;

    [Header("AtCrash")]
    public bool atCrash;
    [SerializeField] private float atCrashTime =3;

    //Script
    private BehaviorTree bt;
    private EnemyFireSystem _fireSystem;
    private Vector3 StartPosition;
    private Quaternion StartRotation;

    //event
    public delegate void ToPlayEnemyHit();
    public event ToPlayEnemyHit OnEnemyHit;
    public delegate void EnemyDeath();
    public event EnemyDeath OnEnemyDeath;
    public event EnemyDeath OnEnemyRebirth;

    //variable
    private float atCrashTimer;
    private bool isTriggerDeath;

    public Core Core { get; private set; }
    public Stats Stats { get; private set; }
    public Combat Combat { get; private set; }
    public Movement Movement { get; private set; }
    public CollisionSenses CollisionSenses { get; private set; }

    private float notGroundedTimer;

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        Stats = Core.GetCoreComponent<Stats>();
        Combat = Core.GetCoreComponent<Combat>();
        Movement = Core.GetCoreComponent<Movement>();
        CollisionSenses = Core.GetCoreComponent<CollisionSenses>();

        _fireSystem = GetComponent<EnemyFireSystem>();

        notGroundedTimer = 0f;
    }

    private void OnEnable()
    {
        Stats.Health.OnValueDecreased += Health_OnValueDecreased;
        Stats.Health.OnValueChanged += Health_OnValueChanged;
        Stats.OnBurnChanged += Stats_OnBurnChanged;

        StartPosition = this.transform.position;
        StartRotation = this.transform.rotation;
    }
    private void Start()
    {
        bt = GetComponent<BehaviorTree>();

        Stats.Health.Init();
        Stats.SetOnFire(false);
        Stats_OnBurnChanged(false);

        GameManager.Instance.OnPlayerReborn += RebirthSelf;
    }
    private void OnDisable()
    {
        Stats.Health.OnValueDecreased -= Health_OnValueDecreased;
        Stats.Health.OnValueChanged -= Health_OnValueChanged;
        Stats.OnBurnChanged -= Stats_OnBurnChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerReborn -= RebirthSelf;
    }

    private void Health_OnValueDecreased()
    {
        if(Stats.Health.CurrentValuePercentage > 0.95f)
        {
            return;
        }

        if (bt != null)
        {
            bt.SendEvent("HitByPlayer");
        }
        OnEnemyHit?.Invoke();

        if (_fireSystem != null)
        {
            _fireSystem.FireCheck();
        }
    }




    private void Update()
    {
        Core.LogicUpdate();

        AtCrashTimerSystem();

        if (gameObject.name.Contains("A"))
        {
            if (CollisionSenses.Ground)
            {
                notGroundedTimer = 0f;
            }
            else
            {
                notGroundedTimer += Time.deltaTime;
                if (notGroundedTimer > 10f)
                {
                    EnemyDeathRightNow();
                }
            }
        }
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();
    }

    private void FixedUpdate()
    {
        Core.PhysicsUpdate();
    }
    private void Health_OnValueChanged()
    {
        GealthFeedbackByHP(Stats.Health.CurrentValuePercentage);
    }

    private void Stats_OnBurnChanged(bool value)
    {
        if (value)
        {
            feedbacks_Fire.PlayFeedbacks();
        }
        else
        {
            feedbacks_Fire.StopFeedbacks();
        }
    }

    public void SetIsRebirthHide(bool value)
    {
        isRebirthHide = value;
    }

    public void GiveTargetPlayer()
    {
        GameObject player = GameManager.Instance.Player.gameObject;
        if (TryGetComponent(out EnemyAggroSystem enemyAggroSystem)) 
            enemyAggroSystem.GiveAggroTarget(player);
    }

    private void AtCrashTimerSystem()
    {
        if(atCrash)
        {
            atCrashTimer += Time.deltaTime;
        }

        if(atCrashTimer>atCrashTime)
        {
            SetAtCrash(false);
            atCrashTimer = 0;
        }
    }

    #region Feedback
    private void GealthFeedbackByHP(float healthPercentage)
    {
        if (healthPercentage == 0)
        {
            if (!isTriggerDeath)
            {
                EnemyDeathRightNow();
            }
        }
        else if(healthPercentage < 1f / 6f)
        {
            isShockVFX = true;

            if (this.transform.gameObject != null)
            {
                feedbacks_Shock.PlayFeedbacks();
            }

        }
        else if(healthPercentage < 2f / 6f)
        {
            feedbacks_Steam.StopFeedbacks();

            if (isShockVFX)
            {
                isShockVFX = false;
                feedbacks_Shock.StopFeedbacks();
            }
        }
        else if(healthPercentage < 3f / 6f)
        {
            _eye.SetPurple();

            if (!isSteam)
            {
                isSteam = true;
                feedbacks_Steam.PlayFeedbacks();
            }

            if (isFireVFX)
            {
                feedbacks_Steam.PlayFeedbacks();
            }
        }
        else if(healthPercentage < 4f / 6f)
        {
            isSteam = true;

            _eye.SetRed();
            feedbacks_Steam.PlayFeedbacks();
        }
        else if(healthPercentage < 5f / 6f)
        {
            isHurt = true;

            _eye.SetOrange();

            if (isSteam)
            {
                isSteam = false;
                feedbacks_Steam.StopFeedbacks();
            }
        }
        else
        {
             _eye.SetYellow();

            if (isHurt)
            {
                isHurt = false;
                feedbacks_Steam.StopFeedbacks();
            }
        }
    }
    private void BoomBody()
    {
        if (VFX_Death == null) return;
        Vector3 offset = new(0, 1, 0);
        GameObject VFX = Instantiate(VFX_Death, this.transform.position+ offset, Quaternion.identity);
        Destroy(VFX, 3.5f);
    }
    #endregion
    #region
    public void Rebirth(Vector3 position, Quaternion rotation)
    {
        OnEnemyRebirth?.Invoke();
        transform.position = position;
        transform.rotation = rotation;
        Initialization();
    }
    private void RebirthSelf()
    {
        if(this.transform.gameObject !=  null)
        {
            Rebirth(StartPosition, StartRotation);
        }
    }

    private void Initialization()
    {
        if(isOnceEnemy)
        {
            this.gameObject.SetActive(false);
            return;
        }

        EnemyAggroSystem aggroSystem = GetComponent<EnemyAggroSystem>();

        Stats.Health.Init();
        Stats.SetOnFire(false);
        gameObject.SetActive(true);

        if (aggroSystem != null)
        {
            aggroSystem.CleanAggroTarget();
        }

        if(!isRebirthHide)
        {
            this.gameObject.SetActive(true);
        }
        isHurt = false;
        isSteam = false;
        isFireVFX = false;
        isShockVFX = false;
        Boom = false;
        _eye.SetYellow();
        feedbacks_Steam.StopFeedbacks();
        feedbacks_Fire.StopFeedbacks();
        feedbacks_Shock.StopFeedbacks();
        feedbacks_Boom.StopFeedbacks();
        feedbacks_FlyBoom.StopFeedbacks();
        SetIsTriggerDeath(false);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(Boom)
        {
            GameObject spreadObj = Instantiate(spreadArea, this.transform.position, Quaternion.identity);
            Destroy(spreadObj, 1.5f);
            feedbacks_FlyBoom.PlayFeedbacks();
            bt.enabled = false;
        }
    }

    public void SetAtCrash(bool active)
    {
        atCrash = active;
    }
    public void EnemyDeathRightNow()
    {
        SetIsTriggerDeath(true);

        feedbacks_Boom.PlayFeedbacks();
        BoomBody();
        feedbacks_Fire.StopFeedbacks();
        feedbacks_Shock.StopFeedbacks();
        feedbacks_Steam.StopFeedbacks();
        OnEnemyDeath?.Invoke();
    }
    private void SetIsTriggerDeath(bool value)
    {
        isTriggerDeath = value;
    }
}
