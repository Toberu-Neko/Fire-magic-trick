using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }
    [field: SerializeField] public PlayerInputHandler InputHandler { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }

    public PlayerIdleState IdleState { get; private set; }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, Data, "idle");
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void AnimationActionTrigger() => StateMachine.CurrentState.AnimationActionTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void AnimationStartMovementTrigger() => StateMachine.CurrentState.AnimationStartMovementTrigger();

    private void AnimationStopMovementTrigger() => StateMachine.CurrentState.AnimationStopMovementTrigger();

    private void AnimationSFXTrigger() => StateMachine.CurrentState.AnimationSFXTrigger();

}
