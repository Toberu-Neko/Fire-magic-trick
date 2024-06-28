using UnityEngine;

public class State_Patrol : EnemyFSMBaseState
{
    //reference
    private ED_EnemyPatrol data;
    private Transform enemy;

    //value
    private float speed;

    public State_Patrol(Entity entity, EnemyStateMachine stateMachine, ED_EnemyPatrol enemyData, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        this.data = enemyData;
        enemy = entity.transform;
    }
    public override void Enter()
    {
        base.Enter();
        speed = data.PatrolSpeed;
        Movement.SetVelocity(speed, enemy.forward);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Navigation.isObstacleForward)
        {
            Movement.SetVelocityZero();
            Movement.RotateIncrease(5);
        }
        else
        {
            Movement.SetVelocity(speed, enemy.forward);
        }
    }
}
