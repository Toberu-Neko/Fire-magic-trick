using UnityEngine;

public class StateController : MonoBehaviour
{
    private State currentState;

    //State
    public E_Sleep sleepState = new();
    public E_Patrol patrolState = new();
    public E_Hurt hurtState = new();
    public E_Chase chaseState = new();
    public E_Attack attackState = new();

    private void Start()
    {
        ChangeState(sleepState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = newState;
        currentState.OnStateEnter(this);
    }
}
