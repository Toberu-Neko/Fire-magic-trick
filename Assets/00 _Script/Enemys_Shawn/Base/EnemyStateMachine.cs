using MoreMountains.Feedbacks;
using System;

[Serializable]
public class EnemyStateMachine
{
    [MMFReadOnly]public string CurrentStateName;
    public EnemyFSMBaseState CurrentState { get; private set; }

    private bool canChangeState = true;

    public void Initialize(EnemyFSMBaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
        CurrentStateName = startingState.GetType().Name;
    }
    public void ChangeState(EnemyFSMBaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentStateName = newState.GetType().Name;
        CurrentState.Enter();
    }
    public void SetCanChangeState(bool canChangeState)
    {
        this.canChangeState = canChangeState;
    }
}
