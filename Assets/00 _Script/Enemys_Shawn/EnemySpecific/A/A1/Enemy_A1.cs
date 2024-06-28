using UnityEngine;

public class Enemy_A1 : Enemy_A
{
    //Base setting for enemy class.
    [field: SerializeField] public EA1_StateData Data { get; private set; }
    
    //Animation

    //general state for enemys.
    public EA1_State_Idle S_Idel { get; private set; }
    public EA1_State_Patrol S_Patrol { get; private set; }

    /*
    public EA1_State_Alert S_Alert { get; private set; }
    public EA1_State_Attack S_Attack { get; private set; }
    public EA1_State_Death S_Death { get; private set; }
    */

    public override void Awake()
    {
        base.Awake();

        //State
        S_Idel = new EA1_State_Idle(this, StateMachine, Data.IdleData, "idel");
        S_Patrol = new EA1_State_Patrol(this, StateMachine, Data.PatrolData, "patrol");

        /*
        S_Alert = new EA1_State_Alert(this, StateMachine, Data, "alert");
        S_Attack = new EA1_State_Attack(this, StateMachine, Data, "attack");
        S_Death = new EA1_State_Death(this, StateMachine, Data, "death");
        */
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
