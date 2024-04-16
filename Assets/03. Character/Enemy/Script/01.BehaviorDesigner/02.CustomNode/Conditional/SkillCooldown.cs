using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SkillCooldown : Conditional
{
    [Header("CooldownTime")]
    [SerializeField] private float cooldown;
    [SerializeField] private float randomOffset;

    [HideInInspector] public float timer;
	private float CooldownAfterRandom;

    public override TaskStatus OnUpdate()
    {
        if (Time.time - timer > CooldownAfterRandom)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    public void OffsetCooldown()
    {
		CooldownAfterRandom = cooldown;
        CooldownAfterRandom += Random.Range(0, randomOffset);
    }
}