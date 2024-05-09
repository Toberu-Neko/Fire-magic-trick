using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WaterBulletCountCheck : Conditional
{
    [Header("SharedVariable")]
    [SerializeField] private SharedInt waterBulletCount;

    [Header("Conditional")]
    [SerializeField] private bool useMax = false;
    [SerializeField] private int max;
    [SerializeField] private bool useMin = false;
    [SerializeField] private int min;

    public override void OnStart()
    {

    }
    public override TaskStatus OnUpdate()
    {
        if (isWithinRange())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    private bool isWithinRange()
    {
        if (!useMax)
        {
            if (min <= waterBulletCount.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (!useMin)
        {
            if (waterBulletCount.Value <= max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (min <= waterBulletCount.Value && waterBulletCount.Value <= max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}