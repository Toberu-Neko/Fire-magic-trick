using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SohaStateCheck : Conditional
{
    [Header("SharedVariable")]
    [SerializeField] private SharedGameObject sohaObject;

    [Header("State")]
    [SerializeField] private Soha.State targetState;
    
    private Soha soha;

    public override void OnStart()
    {
        soha = sohaObject.Value.GetComponent<Soha>();
    }

    public override TaskStatus OnUpdate()
    {
        if(soha.state == targetState)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}