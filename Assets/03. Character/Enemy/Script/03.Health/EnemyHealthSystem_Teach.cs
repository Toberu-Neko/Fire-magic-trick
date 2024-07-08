using MoreMountains.Feedbacks;
using UnityEngine;
using System.Threading.Tasks;

public class EnemyHealthSystem_Teach : MonoBehaviour
{
    //TODO: �o�ݰ_�ӨS�b��

    [Header("State")]
    public bool isIgnite;
    public bool isHurt;
    public bool isSteam;
    public bool isFire;
    public bool isShock;
    public bool Boom;

    [Header("Health")]
    [SerializeField] private int StartHealth;
    public int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int ignitionPoint;

    [Header("Cooling")]
    [SerializeField] private float coolingInterval;
    [SerializeField] private float coolingTime;

    [Header("Feedbacks")]
    [SerializeField] private EyeColorController _eye;
    [SerializeField] private MMF_Player feedbacks_Steam;
    [SerializeField] private MMF_Player feedbacks_Fire;
    [SerializeField] private MMF_Player feedbacks_Shock;
    [SerializeField] private MMF_Player feedbacks_Boom;
    [SerializeField] private MMF_Player feedbacks_FlyBoom;

    private ProgressSystem _progress;
    private Transform startPosition;
    private float hitTimer;
    private float coolingTimer;
    private bool isCooling;
    private bool isInterval;
    public int iHealth
    {
        get { return health; }
        set { health = value; }
    }
    private void Awake()
    {
        health = maxHealth;
    }
    private void Start()
    {
        _progress = GameManager.Instance.GetComponent<ProgressSystem>();
        startPosition = this.transform;

    }
    private void Update()
    {
        EnemyCoolingCheck();
    }
    #region Cooling
    private void EnemyCoolingCheck()
    {
        isCooling = Time.time - hitTimer > coolingTime ? true : false;
        isInterval = Time.time - coolingTimer > coolingInterval ? true : false;

        if (health < maxHealth && isCooling && isInterval)
        {
            EnemyCooling();
        }
    }
    private void EnemyCooling()
    {
        health++;
        coolingTimer = Time.time;
        healthFeedback(health);
        Debug.Log("Enemy remain health" + health);
    }
    #endregion
    #region Damage
    public void TakeDamage(int damage , PlayerDamage.DamageType damageType)
    {
        health -= damage;
        Debug.Log(health);
        hitTimer = Time.time;

        healthFeedback(health);
        Debug.Log("Enemy remain health" + health);

        if (health <= 0)
        {
            EnemyDie();
        }
        else if (health <= ignitionPoint)
        {
            EnemyIgnite();
        }
    }
    private void EnemyIgnite()
    {
        isIgnite = true;
    }
    #endregion
    #region Feedback
    private void healthFeedback(int health)
    {
        if(health == 6)
        {
            _eye.SetYellow();

            if(isHurt)
            {
                isHurt = false;
            }
        }
        if (health == 5) 
        {
            isHurt = true;

            _eye.SetOrange();

            if(isSteam)
            {
                isSteam = false;
                feedbacks_Steam.StopFeedbacks();
            }
        }
        if (health == 4)
        {
            isSteam = true;

            _eye.SetRed();
            feedbacks_Steam.PlayFeedbacks();
        }
        if (health == 3)
        {
            _eye.SetPurple();

            if(!isSteam)
            {
                isSteam = true;
                feedbacks_Steam.PlayFeedbacks();
            }

            if(isFire)
            {
                isFire = false;
                feedbacks_Steam.PlayFeedbacks();
                feedbacks_Fire.StopFeedbacks();
            }
        }
        if (health == 2)
        {
            isFire = true;

            feedbacks_Steam.StopFeedbacks();
            feedbacks_Fire.PlayFeedbacks();

            if(isShock)
            {
                isShock = false;
                feedbacks_Shock.StopFeedbacks();
            }
        }
        if (health == 1)
        {
            isShock = true;

            feedbacks_Shock.PlayFeedbacks();

            if(!isFire)
            {
                isFire = true;
                feedbacks_Fire.PlayFeedbacks();
            }
        }
        if (health == 0 || health<0)
        {
            feedbacks_Boom.PlayFeedbacks();
            feedbacks_Fire.StopFeedbacks();
            feedbacks_Shock.StopFeedbacks();
            feedbacks_Steam.StopFeedbacks();
        }
    }
    #endregion
    #region
    private async void EnemyDie()
    {
        await Task.Delay(1500);
        //this.gameObject.SetActive(false);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(Boom)
        {
            feedbacks_FlyBoom.PlayFeedbacks();
        }
    }
}
