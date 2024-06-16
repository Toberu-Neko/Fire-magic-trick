using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody RB { get; private set; }
    public Transform ParentTransform { get; private set; }

    private Vector3 velocityWorkspace;
    public Vector3 CurrentVelocity { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ParentTransform = core.transform.parent;
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

    public void SetVelocity(float velocity, Vector2 V2Direction)
    {
        velocityWorkspace.Set(V2Direction.x * velocity, CurrentVelocity.y, V2Direction.y * velocity);

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

    public void Rotate(float value)
    {
        ParentTransform.rotation = Quaternion.Euler(0f, value, 0f);
    }

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

}
