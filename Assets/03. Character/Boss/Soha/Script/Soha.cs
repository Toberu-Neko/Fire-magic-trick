using MoreMountains.Feedbacks;
using UnityEngine;

public class Soha : MonoBehaviour,IHealth
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
    [SerializeField] private Satun_Laser_Manager laser_Manager;
    [Header("Death")]
    [SerializeField] private TriggerArea_Timeline End;
    [SerializeField] private GameObject FinishRoead;
    
    //Script
    private Boss_System system;

    //Value
    public int iHealth { get; set; }
    
    private void Awake()
    {
        system = GetComponent<Boss_System>();
    }
    private  void Start()
    {
        Initialization();
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
    public void TakeDamage(int damage, PlayerDamage.DamageType damageType)
    {
        health -= damage;
        stateEventCheck();
        if (state == State.Death) return; 

        CheckActive();

        system.SetHealth(healthPersentage(health));
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
        else if (health < 100)
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
        laser_Manager.playLaser();
    }
    private void Event_Death()
    {
        changeState(State.Death);
        eventTrigger(State.Death);
        system.EndBossFight();
        End.readyPlay();
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
                break;
        }
    }
    private float healthPersentage(float newHealth)
    {
        if (newHealth < 0 || newHealth > Health_Full) return 0;

        float persenHealth = newHealth / Health_Full;
        return persenHealth;
    }
}
