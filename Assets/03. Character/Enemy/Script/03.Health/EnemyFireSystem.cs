using UnityEngine;

public class EnemyFireSystem : MonoBehaviour
{
    [SerializeField] private GameObject TrackTarget;
    [SerializeField] private GameObject DashFire;
    [SerializeField] private GameObject SuperDashFire;
    [Header("Setting")]
    [SerializeField] private float spreadTimer;


    private EnemyHealthSystem health;

    private float timerNumber;
    private bool isSpread;
    private bool isTimer;

    private void Awake()
    {
        health = GetComponent<EnemyHealthSystem>();
    }

    private void OnEnable()
    {
        health.OnEnemyRebirth += Initialized;
        Initialized();
    }

    private void OnDisable()
    {
        health.OnEnemyRebirth -= Initialized;
    }

    private void Update()
    {
        TrackTargetSystem();
        SpreadTimerSystem();
    }
    private void Initialized()
    {
        SetIsTimer(false);
        SetTimerNumber(0);
        SetIsSpread(false);
        SetDashFire(false);
        SetSuperDashFire(false);
    }

    public void FireCheck()
    {

        if(Random.Range(0, 2) == 0)
        {
            SetDashFire(true);
        }
        else
        {
            SetSuperDashFire(true);
        }

        SetIsSpread(true);
        SetTrackTarget(true);
        SetIsTimer(true);
        SetTimerNumber(spreadTimer);
    }

    private void TrackTargetSystem()
    {
        if (!isSpread)
        {
            if (health.Stats.IsBurning)
            {
                SetTrackTarget(true);
            }
            else
            {
                SetTrackTarget(false);
            }
        }
    }
    private void SpreadTimerSystem()
    {
        if(isTimer)
        {
            timerNumber -= Time.deltaTime;
        }

        if(timerNumber <=0 && isTimer)
        {
            Initialized();
        }
    }
    private void SetIsTimer(bool active)
    {
        isTimer = active;
    }
    private void SetTimerNumber(float numbber)
    {
        timerNumber = numbber;
    }
    private void SetDashFire(bool active)
    {
        if(DashFire!=null)
        {
            DashFire.SetActive(active);
        }
    }
    private void SetSuperDashFire(bool active)
    {
        SuperDashFire.SetActive(active);
    }
    private void SetIsSpread(bool active)
    {
        isSpread = active;
    }
    private void SetTrackTarget(bool active)
    {
        TrackTarget.SetActive(active);
    }
}
