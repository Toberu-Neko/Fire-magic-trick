public class EnemyStateMachine
{
    public EnemyFSMBaseState CurrentState { get; private set; }

    public void Initialize(EnemyFSMBaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(EnemyFSMBaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
