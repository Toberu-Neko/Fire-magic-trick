using BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;
using UnityEngine;

public class EnemyNavigation : CoreComponent
{
    [Header("Reference")]
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Value")]
    [SerializeField] private float ObstacleDetectionRange = 1f;
    [SerializeField] private float ObstacleDetectionRadius = 0.5f;

    private Vector3 halfExtents;
    private RaycastHit hit;

    protected override void Awake()
    {
        base.Awake();
        halfExtents = new Vector3(ObstacleDetectionRadius, ObstacleDetectionRadius, ObstacleDetectionRadius);
    }

    public bool isObstacleForward
    {
        get
        {
            return Physics.BoxCast(eyeTransform.position, halfExtents, eyeTransform.forward,out hit, Quaternion.identity, ObstacleDetectionRange, obstacleLayer);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = isObstacleForward? Color.red : Color.green; // if there is an obstacle, color is red, otherwise green.
        if(isObstacleForward)
        {
            Gizmos.DrawRay(eyeTransform.position, eyeTransform.forward * hit.distance);
            Gizmos.DrawWireCube(eyeTransform.position + eyeTransform.forward * hit.distance, halfExtents * 2);
        }
        else
        {
            Gizmos.DrawRay(eyeTransform.position, eyeTransform.forward * ObstacleDetectionRange);
            Gizmos.DrawWireCube(eyeTransform.position + eyeTransform.forward * ObstacleDetectionRange, halfExtents * 2);
        }
    }
}
