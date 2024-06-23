public class EnemyStateMachine
{
    public EnemyFSMBaseState CurrentState { get; private set; }

    private bool canChangeState = true;

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
    public void SetCanChangeState(bool canChangeState)
    {
        this.canChangeState = canChangeState;
    }
}
