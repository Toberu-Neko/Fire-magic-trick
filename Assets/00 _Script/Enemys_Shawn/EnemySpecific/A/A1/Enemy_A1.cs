using UnityEngine;

public class Enemy_A1 : Entity
{
    //Base setting for enemy class.
    [field: SerializeField] public EnemyData Data { get; private set; }

    //Animation

    //general state for enemys.
    public EA1_State_Idel S_Idel { get; private set; }
    public E_State_Alert S_Alert { get; private set; }
    public E_State_Attack S_Attack { get; private set; }
    public E_State_Patrol S_Patrol { get; private set; }
    public E_State_OnHit S_OnHit { get; private set; }
    public E_State_Death S_Death { get; private set; }

    public override void Awake()
    {
        base.Awake();

        //State
        S_Idel = new EA1_State_Idel(this, StateMachine, Data, "idel");
        S_Alert = new E_State_Alert(this, StateMachine, Data, "alert");
        S_Attack = new E_State_Attack(this, StateMachine, Data, "attack");
        S_Patrol = new E_State_Patrol(this, StateMachine, Data, "patrol");
        S_OnHit = new E_State_OnHit(this, StateMachine, Data, "onHit");
        S_Death = new E_State_Death(this, StateMachine, Data, "death");
    }
    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(S_Idel);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
