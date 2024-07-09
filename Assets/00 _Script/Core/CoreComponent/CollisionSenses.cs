using Unity.VisualScripting;
using UnityEngine;
public class CollisionSenses : CoreComponent
{
    private Movement movement;

    public Transform GroundCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(groundCheck, transform.parent.name);
        private set => groundCheck = value;
    }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 groundCheckV3;
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private float longGroundCheckDistance = 1.25f;
    [SerializeField] private float slopeCheckDistance = 0.75f;
    [SerializeField] private float longSlopeCheckDistance = 1.5f;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsSlopeDetection;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float maxSlopeAngle = 30f;
    private Slope slope = new();

    protected override void Awake()
    {
        base.Awake();

        movement = core.GetCoreComponent<Movement>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public bool Ground
    {
        get
        {
            if (!Slope.IsOnSlope)
            {
                return Physics.BoxCast(GroundCheck.position, groundCheckV3, Vector3.down, movement.ParentTransform.localRotation, slopeCheckDistance, whatIsGround);
            }
            else
            {
                return Physics.BoxCast(GroundCheck.position, groundCheckV3, Vector3.down, movement.ParentTransform.localRotation, groundCheckDistance, whatIsGround);
            }
            
        }
    }

    public bool LongGround
    {
        get
        {
            return Physics.BoxCast(GroundCheck.position, groundCheckV3, Vector3.down, movement.ParentTransform.localRotation, longGroundCheckDistance, whatIsGround);
        }
    }
    public bool Enemy
    {
        get
        {
            return Physics.BoxCast(GroundCheck.position, groundCheckV3, Vector3.down, movement.ParentTransform.localRotation, slopeCheckDistance, whatIsEnemy);
        }
    }

    public Slope Slope
    {
        get
        {
            slope = new();
            slope.hasCollisionSenses = true;

            RaycastHit[] down = Physics.BoxCastAll(GroundCheck.position, groundCheckV3, Vector3.down, movement.ParentTransform.localRotation, longSlopeCheckDistance, whatIsSlopeDetection);

            foreach (RaycastHit hit in down)
            {
                Vector3 normal = hit.normal;
                float angle = Vector3.Angle(Vector3.up, normal);

                if (angle != 0)
                {
                    slope.Set(normal, angle, hit);
                    slope.SetIsOnSlope(true);
                    // Debug.Log(slope.DownAngle);
                    break;
                }
            }


            return slope;
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        if (GroundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GroundCheck.position, groundCheckV3);
            Gizmos.DrawLine(GroundCheck.position, GroundCheck.position + Vector3.down * groundCheckDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(GroundCheck.position, GroundCheck.position + Vector3.down * slopeCheckDistance);
        }

    }
}

public class Slope
{
    public RaycastHit Hit { get; private set; }
    public Vector3 NormalPrep { get; private set; }
    public float DownAngle { get; private set; }
    public bool IsOnSlope { get; private set; }
    public bool ExceedsMaxSlopeAngle => DownAngle > 30f;
    public bool hasCollisionSenses;

    public Slope()
    {
        NormalPrep = Vector3.zero;
        DownAngle = 0f;
        IsOnSlope = false;
        hasCollisionSenses = false;
    }

    public void SetIsOnSlope(bool isOnSlope)
    {
        IsOnSlope = isOnSlope;
    }

    public void Set(Vector3 slopeNormal, float slopeAngle, RaycastHit hit)
    {
        Hit = hit;
        NormalPrep = slopeNormal;
        DownAngle = slopeAngle;

        IsOnSlope = true;
    }
}
