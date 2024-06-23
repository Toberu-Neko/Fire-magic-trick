using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    protected override void Awake()
    {
        base.Awake();

        ParentTransform = core.transform.parent;
        RB = GetComponentInParent<Rigidbody>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();

        useGravity = RB.useGravity;
    }

    private void OnEnable()
    {
        velocityWorkspace = Vector3.zero;
        V2ToV3Workspace = Vector3.zero;

        wasOnSlope = false;
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

    public void SetVelocity(float velocity, Vector3 direction)
    {
        velocityWorkspace = direction * velocity;

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
        RB.velocity = velocityWorkspace;

        CurrentVelocity = velocityWorkspace;
    }

    public void AddForce(float speed, Vector3 dir)
    {
        RB.AddForce(speed * dir, ForceMode.Acceleration);
    }

    public void Rotate(float value)
    {
        ParentTransform.rotation = Quaternion.Euler(0f, value, 0f);
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
