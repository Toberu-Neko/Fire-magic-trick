using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Playables;

public class Soha : MonoBehaviour, IDamageable
{
    public enum State
    {
        inactive,
        Full,//60-100
        Mid,//30-60
        Low,//0-30
        Death,
    }
    [Header("State")]
    [MMFReadOnly] public State state;
    [Header("State Value")]
    [MMFReadOnly] public float health;
    [SerializeField] private float Health_Full = 100;
    [SerializeField] private float Health_Mid = 60;
    [SerializeField] private float Health_Low = 30;
    [MMFReadOnly] [SerializeField] private float health_Death = 0;

    [Header("Dialogue")]
    [SerializeField] private TriggerArea_DialogueTrigger dialogue_Mid;
    [SerializeField] private TriggerArea_DialogueTrigger dialogue_Low;
    [SerializeField] private TriggerArea_DialogueTrigger dialogue_Death;
    [Header("Univasal")]
    [SerializeField] private SteamBoom boom;
    [Header("Mid")]
    [SerializeField] private GlassRoadManager glassRoadManager;
    [SerializeField] private CardMachineManager  topEnemyManager;
    [Header("Death")]
    [SerializeField] private Timeline_Trigger End;
    [SerializeField] private GameObject FinishRoead;

    [SerializeField] private PlayableDirector endDir;
    //Script
    private Boss_System system;
    
    private void Awake()
    {
        system = GetComponent<Boss_System>();
    }
    private  void Start()
    {
        Initialization();

        system.onResetFight += Initialization;
        system.onStartFight += stateEventCheck;
    }

    private void OnDestroy()
    {
        system.onStartFight -= stateEventCheck;
        system.onResetFight -= Initialization;
    }

    private void Update()
    {
        CheckActive();
    }

    private void Initialization()
    {
        health = Health_Full;
        system.SetHealth(healthPersentage(health));
        topEnemyManager.ToClose();
        if (state != State.Death) state = State.inactive;
    }
    private void CheckActive()
    {
        if (state != State.Death)
        {
            if (health > 0)
            {
                if (this.gameObject.activeSelf == false)
                {
                    this.gameObject.SetActive(true);
                }
            }
        }
    }
    private void stateEventCheck()
    {
        if (health <= 0)
        {
            if(state == State.Low)
            {
                Event_Death();
            }
        }
        else if (health < Health_Low)
        {
            if (state == State.Mid)
            {
                Event_Low();
            }
        }
        else if (health < Health_Mid)
        {
            if (state == State.Full)
            {
                Event_Mid();
            }
        }
        else
        {
            Event_Start();
        }
    }
    private void Event_Start()
    {
        changeState(State.Full);
    }
    private void Event_Mid()
    {
        changeState(State.Mid);
        eventTrigger(State.Mid);
        topEnemyManager.ToSpawn();
        boom.SteamBoomRightNow();
        glassRoadManager.StartAll();
    }
    private void Event_Low()
    {
        changeState(State.Low);
        eventTrigger(State.Low);
        boom.SteamBoomRightNow();
    }
    private void Event_Death()
    {
        changeState(State.Death);
        eventTrigger(State.Death);
        system.EndBossFight();
        FinishRoead.SetActive(true);
    }
    private void changeState(State state)
    {
        this.state = state;
    }
    private void eventTrigger(State state)
    {
        switch (state)
        {
            case State.Full:
                break;

            case State.Mid:
                dialogue_Mid.EventTrigger();
                break;

            case State.Low:
                dialogue_Low.EventTrigger();
                break;

            case State.Death:
                dialogue_Death.EventTrigger();
                endDir.Play();
                break;
        }
    }
    private float healthPersentage(float newHealth)
    {
        if (newHealth < 0 || newHealth > Health_Full) return 0;

        float persenHealth = newHealth / Health_Full;
        return persenHealth;
    }

    public void Damage(float damageAmount, Vector3 damagePosition, bool trueDamage = false)
    {
        health -= damageAmount;
        stateEventCheck();
        if (state == State.Death) return;

        CheckActive();

        system.SetHealth(healthPersentage(health));
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
