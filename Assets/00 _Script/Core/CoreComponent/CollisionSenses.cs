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

    [SerializeField] private LayerMask whatIsGround;

    protected override void Awake()
    {
        base.Awake();

        movement = core.GetCoreComponent<Movement>();
    }

    public bool Ground
    {
        get
        {
            return Physics.BoxCast(GroundCheck.position, groundCheckV3, Vector3.down, movement.ParentTransform.localRotation, groundCheckDistance, whatIsGround);
            /*
            if (!Slope.IsOnSlope)
            {
                return Physics2D.BoxCast(GroundCheck.position, groundCheckV2, 0f, Vector2.down, 0.1f, whatIsGround);
            }
            else
            {
                return Physics2D.BoxCast(GroundCheck.position, slopeCheckV2, 0f, Vector2.down, 0.1f, whatIsGround);
            }
            */
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (GroundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GroundCheck.position, groundCheckV3);
        }
    }
}
