using MoreMountains.Feedbacks;
using UnityEngine;
using BehaviorDesigner.Runtime;

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

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        Stats = Core.GetCoreComponent<Stats>();
        Combat = Core.GetCoreComponent<Combat>();
        Movement = Core.GetCoreComponent<Movement>();

        _fireSystem = GetComponent<EnemyFireSystem>();
    }

    private void OnEnable()
    {
        Stats.Health.OnValueDecreased += Health_OnValueDecreased;
        Stats.Health.OnValueChanged += Health_OnValueChanged;
    }


    private void OnDisable()
    {
        Stats.Health.OnValueDecreased -= Health_OnValueDecreased;
        Stats.Health.OnValueChanged -= Health_OnValueChanged;
    }

    private void Health_OnValueDecreased()
    {
        bt.SendEvent("HitByPlayer");
        OnEnemyHit?.Invoke();

        if (_fireSystem != null)
        {
            _fireSystem.FireCheck();
        }
    }

    private void Health_OnValueChanged()
    {
        GealthFeedback(Stats.Health.CurrentValuePercentage);
    }

    private void Start()
    {
        bt = GetComponent<BehaviorTree>();
        StartPosition = this.transform.position;
        StartRotation = this.transform.rotation;
    }

    private void Update()
    {
        AtCrashTimerSystem();
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
    private void GealthFeedback(float healthPercentage)
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

            if (!isFireVFX)
            {
                isFireVFX = true;
                feedbacks_Fire.PlayFeedbacks();
            }
        }
        else if(healthPercentage < 2f / 6f)
        {
            isFireVFX = true;

            feedbacks_Steam.StopFeedbacks();
            feedbacks_Fire.PlayFeedbacks();

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
                isFireVFX = false;
                feedbacks_Steam.PlayFeedbacks();
                feedbacks_Fire.StopFeedbacks();
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
        this.transform.position = position;
        this.transform.rotation = rotation;
        Initialization();
        return;
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
