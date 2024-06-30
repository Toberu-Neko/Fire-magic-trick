using UnityEngine;

public class State_Patrol : EnemyFSMBaseState
{
    //reference
    private ED_EnemyPatrol data;
    private Transform enemy;

    //value
    private float speed;
    private bool isTurning;
    private float targetAngle;
    private float turnAngle = 90f;

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
        isTurning = false;
        targetAngle = 0f;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Navigation.isObstacleForward || isTurning)
        {
            isTurning = true;

            // �p��o�@�ժ����ਤ��, 1. �ثe��Y����, 2 . �ؼШ���,
            // 3. ����t�� ( 10 �� * 1�ժ��ɶ�, �]�N�O 1/6 ��, ���o�ժ����ਤ�״N�|�O �u (�ؼШ��� - �ثe����) * 0.16�v, �èC�է�s�ثe����, �F��ƶ����ĪG.)
            float targetDegree = Mathf.Lerp(enemy.eulerAngles.y, targetAngle, 10f * Time.deltaTime);
            Movement.Rotate(targetDegree);

            if (turnAngle - targetDegree < 1f)
            {
                isTurning = false;
                Movement.Rotate(targetAngle);
            }

            Movement.SetVelocityZero();
        }
        else
        {
            // �p��ؼШ���, �קK�C�ճ����s�p��
            targetAngle = enemy.eulerAngles.y + turnAngle;
            Movement.SetVelocity(speed, enemy.forward);
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
