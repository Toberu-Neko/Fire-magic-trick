using BehaviorDesigner.Runtime.Tasks;

public class IgniteState : Conditional
{
	public override TaskStatus OnUpdate()
	{
		if(GetComponent<EnemyHealthSystem>().Stats.IsBurning)
		{
			return TaskStatus.Success;
		}
		else
		{
			return TaskStatus.Failure;
		}
	}
}