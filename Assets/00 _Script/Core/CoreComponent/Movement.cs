using System.Collections;
using UnityEngine;

public class Movement : CoreComponent
{
    private CollisionSenses collisionSenses;

    public Rigidbody RB { get; private set; }
    public Transform ParentTransform { get; private set; }

    private Vector3 velocityWorkspace;
    private Vector3 V2ToV3Workspace;
    public Vector3 CurrentVelocity { get; private set; }
    public float CurrentVelocityXZMagnitude => new Vector2(CurrentVelocity.x, CurrentVelocity.z).magnitude;
    private float gravityWorkspace;
    private bool useGravity;
    private bool wasOnSlope;
    public bool CanSetVelocity { get; private set; } = true;

    protected override void Awake()
    {
        base.Awake();

        ParentTransform = core.transform.parent;
        RB = GetComponentInParent<Rigidbody>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();

        if (RB != null)
        {
            useGravity = RB.useGravity;
        }
        else
        {
            useGravity = false;
        }
    }

    private void OnEnable()
    {
        velocityWorkspace = Vector3.zero;
        V2ToV3Workspace = Vector3.zero;

        wasOnSlope = false;
        CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CurrentVelocity = RB.velocity;

        if (collisionSenses.Slope.hasCollisionSenses)
        {
            if ((collisionSenses.Slope.IsOnSlope && !collisionSenses.Slope.ExceedsMaxSlopeAngle) && collisionSenses.Ground)
            {
                wasOnSlope = true;
                SetGravityZero();
            }
            else if(wasOnSlope)
            {
                wasOnSlope = false;
                SetGravityOrginal();
            }
        }
    }

    public void SetVelocity(float velocity, Vector3 v3Direction, bool ignoreSlope = false)
    {
        velocityWorkspace = v3Direction * velocity;

        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 V2Direction, bool ignoreSlope = false)
    {
        velocityWorkspace.Set(V2Direction.x * velocity, CurrentVelocity.y, V2Direction.y * velocity);

        if ((collisionSenses.Slope.IsOnSlope && !collisionSenses.Slope.ExceedsMaxSlopeAngle) && !ignoreSlope && collisionSenses.Slope.NormalPrep != Vector3.zero && collisionSenses.Ground)
        {
            SetVelocity(velocity, GetSlopeMoveDirection(V2ToV3(V2Direction)));
            return;
        }

        SetFinalVelocity();
    }
    public void SetVelocity(Vector2 velocity, bool ignoreSlope = false)
    {
        //ignore y.
        if ((collisionSenses.Slope.IsOnSlope && !collisionSenses.Slope.ExceedsMaxSlopeAngle) && !ignoreSlope && collisionSenses.Slope.NormalPrep != Vector3.zero && collisionSenses.Ground)
        {
            SetVelocity(velocity.magnitude, GetSlopeMoveDirection(V2ToV3(velocity.normalized)));
            return;
        }

        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(CurrentVelocity.x, velocity, CurrentVelocity.z);

        SetFinalVelocity();
    }

    public void SetVelocityZero()
    {
        velocityWorkspace = Vector3.zero;

        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        if(!CanSetVelocity)
        {
            return;
        }

        RB.velocity = velocityWorkspace;
        CurrentVelocity = velocityWorkspace;
    }
    public void SetCanSetVelocity(bool a)
    {
        CanSetVelocity = a;
    }

    public void AddForce(float speed, Vector3 dir)
    {
        RB.AddForce(speed * dir, ForceMode.Acceleration);
    }

    public void Rotate(float value)
    {
        // Debug.Log("Rotate");
        RB.MoveRotation(Quaternion.Euler(0f, value, 0f));
    }

    public void RotateAdd(float value)
    {
        RB.MoveRotation(Quaternion.Euler(0f, RB.transform.eulerAngles.y + value, 0f));
    }

    /*
    public void RotateSmooth(float value, float time)
    {
        StopCoroutine(nameof(SmoothRotate));
        StartCoroutine(SmoothRotate(value, time));
    }

    private IEnumerator SmoothRotate(float value, float smoothTime)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = ParentTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(ParentTransform.rotation.x, ParentTransform.rotation.y + value, ParentTransform.rotation.z);

        while (elapsedTime < smoothTime)
        {
            ParentTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / smoothTime);

            elapsedTime += Time.deltaTime;

            yield return Time.deltaTime;
        }

        ParentTransform.rotation = targetRotation;
    }
    */

    public void RotateIncrease(float value)
    {
        var rotation = ParentTransform.rotation;
        ParentTransform.rotation *= Quaternion.Euler(rotation.x, rotation.y + value, rotation.z);
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, collisionSenses.Slope.Hit.normal).normalized;
    }

    public Vector3 V2ToV3(Vector2 direction)
    {
        V2ToV3Workspace.Set(direction.x, 0f, direction.y);
        return V2ToV3Workspace;
    }

    #region Set Gravity
    public void SetGravityZero()
    {
        if (useGravity)
        {
            RB.useGravity = false;
        }
    }

    public void SetGravityOrginal()
    {
        if (useGravity)
        {
            RB.useGravity = true;
        }
    }

    public void SetIsKinematic(bool value)
    {
        RB.isKinematic = value;
    }

    #endregion

    /*
    public void Rotate(Vector3 direction)
    {
        ParentTransform.rotation = Quaternion.LookRotation(direction);
    }

    public void Rotate(Vector3 direction, float time)
    {
        StopCoroutine(nameof(RotateIE));
        StartCoroutine(RotateIE(direction, time));
    }

    private IEnumerator RotateIE(Vector3 direction, float time)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = ParentTransform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (elapsedTime < time)
        {
            ParentTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / time);

            elapsedTime += Time.deltaTime;

            yield return Time.deltaTime;
        }

        ParentTransform.rotation = targetRotation;
    }
    */
}
