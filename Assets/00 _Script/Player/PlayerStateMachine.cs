using UnityEngine;

public class PlayerStateMachine
{
    public PlayerFSMBaseState CurrentState { get; private set; }

    public void Initialize(PlayerFSMBaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerFSMBaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;

        // Debug.Log(newState.ToString());
        CurrentState.Enter();
    }
}
