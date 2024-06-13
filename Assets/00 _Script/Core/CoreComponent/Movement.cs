using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody RB { get; private set; }

    private Vector3 velocityWorkspace;
    public Vector3 CurrentVelocity { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody>();
    }

    private void OnEnable()
    {
        velocityWorkspace = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CurrentVelocity = RB.velocity;
    }

    public void SetVelocity(float velocity, Vector3 direction)
    {
        velocityWorkspace = direction * velocity;

        SetFinalVelocity();
    }

    public void SetVelocity(Vector2 VectorVelocity)
    {
        velocityWorkspace = VectorVelocity;

        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity, bool ignoreSlope = false)
    {
        velocityWorkspace.Set(velocity, CurrentVelocity.y, CurrentVelocity.z);

        /*
        if (Slope.IsOnSlope && !ignoreSlope && Slope.NormalPrep != Vector2.zero)
        {
            SetVelocity(velocity, -Slope.NormalPrep);
            return;
        }
        */

        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(CurrentVelocity.x, velocity, CurrentVelocity.z);

        SetFinalVelocity();
    }

    public void SetVelocityZ(float velocity)
    {
        velocityWorkspace.Set(CurrentVelocity.x, CurrentVelocity.y, velocity);

        SetFinalVelocity();
    }

    public void SetVelocityZero()
    {
        velocityWorkspace = Vector2.zero;

        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        RB.velocity = velocityWorkspace;

        CurrentVelocity = velocityWorkspace;
    }

}
