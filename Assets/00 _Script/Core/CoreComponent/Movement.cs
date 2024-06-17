using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Movement : CoreComponent
{
    public Slope Slope { get; set; }

    public Rigidbody RB { get; private set; }
    public Transform ParentTransform { get; private set; }

    private Vector3 velocityWorkspace;
    private Vector3 V2ToV3Workspace;
    public Vector3 CurrentVelocity { get; private set; }
    private float gravityWorkspace;
    private bool useGravity;

    protected override void Awake()
    {
        base.Awake();

        ParentTransform = core.transform.parent;
        RB = GetComponentInParent<Rigidbody>();

        useGravity = RB.useGravity;
    }

    private void OnEnable()
    {
        Slope = new();

        velocityWorkspace = Vector3.zero;
        V2ToV3Workspace = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CurrentVelocity = RB.velocity;

        if (Slope.hasCollisionSenses)
        {
            if (Slope.IsOnSlope)
            {
                SetGravityZero();
            }
            else
            {
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

        if (Slope.IsOnSlope && !ignoreSlope && Slope.NormalPrep != Vector3.zero)
        {
            SetVelocity(velocity, GetSlopeMoveDirection(V2ToV3(V2Direction)));
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

    public void Rotate(float value)
    {
        ParentTransform.rotation = Quaternion.Euler(0f, value, 0f);
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, Slope.Hit.normal).normalized;
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
