using MoreMountains.Tools;
using UnityEngine;

public enum E_State
{
    Idel,
    Alert,
    Attack,
    Patrol,
    OnHit,
    Death
}
public class Enemy : MonoBehaviour
{
    [MMReadOnly][SerializeField] private E_State e_State;
    public void changeState_Enum(E_State state) { e_State = state;}

    //Base setting for enemy class.
    public EnemyStateMachine StateMachine { get; private set; }
    [field: SerializeField] public EnemyData Data { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Core Core { get; private set; }

    //Animation

    //general state for enemys.
    public E_State_Idel S_Idel { get; private set; }
    public E_State_Alert S_Alert { get; private set; }
    public E_State_Attack S_Attack { get; private set; }
    public E_State_Patrol S_Patrol { get; private set; }
    public E_State_OnHit S_OnHit { get; private set; }
    public E_State_Death S_Death { get; private set; }

    private void Awake()
    {

        //CoreComponent

        //State Machine
        StateMachine = new EnemyStateMachine();
        //State
        S_Idel = new E_State_Idel(this, StateMachine, Data, "idel");
        S_Alert = new E_State_Alert(this, StateMachine, Data, "alert");
        S_Attack = new E_State_Attack(this, StateMachine, Data, "attack");
        S_Patrol = new E_State_Patrol(this, StateMachine, Data, "patrol");
        S_OnHit = new E_State_OnHit(this, StateMachine, Data, "onHit");
        S_Death = new E_State_Death(this, StateMachine, Data, "death");
    }
    private void Start()
    {
        StateMachine.Initialize(S_Idel);
    }

    private void Update()
    {
        Core.LogicUpdate();

        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        Core.PhysicsUpdate();

        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();
    }

}
