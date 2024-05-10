using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SohaStateCheck : Conditional
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject sohaObject;

    [Header("State")]
    [SerializeField] private Soha.State targetState;

    [Header("Reverse")]
    [SerializeField] private bool reverse;

    
    private Soha soha;

    public override void OnStart()
    {
        soha = sohaObject.Value.GetComponent<Soha>();
    }

    public override TaskStatus OnUpdate()
    {
        if(!reverse)
        {
            if(soha.state == targetState)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
        else
        {
            if(soha.state == targetState)
            {
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
    }
}