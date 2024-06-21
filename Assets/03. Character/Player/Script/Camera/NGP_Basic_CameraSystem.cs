using UnityEngine;

public class NGP_Basic_CameraSystem : MonoBehaviour
{
    //Script
    protected SuperDashCameraCheck superDashCameraCheck;
    protected AimSupportSystem aimSupport;
    protected ControllerInput input;
    protected PlayerState state;

    //variable
    protected bool isTriggerButton;
    protected bool isLookTarget;
    protected bool isMouse;

    protected virtual void Start()
    {
        superDashCameraCheck = GameManager.Instance.EnergySystem.GetComponent<SuperDashCameraCheck>();
        aimSupport = GameManager.Instance.Player.GetComponent<AimSupportSystem>();
        input = GameManager.Instance.Player.GetComponent<ControllerInput>();
        state = GameManager.Instance.Player.GetComponent<PlayerState>();
    }
    protected virtual void Update()
    {
        if (isMouse) return;
        LockEnemy();
    }
    private void LockEnemy()
    {
        if(!isTriggerButton )
        {
            if(input.RSB)
            {
                setIsTriggerButton(true);

                if (aimSupport.target != null)
                {
                    if (superDashCameraCheck.Target == null)
                    {
                        ClearTarget();
                        setIsLookTarget(false);
                    }
                    ClearTarget();
                    setIsLookTarget(false);
                }
                else if (superDashCameraCheck.Target != null)
                {
                    LockToTarget();
                    setIsLookTarget(true);
                }
                else
                {
                    LookForward();
                }
            }
        }

        if(isLookTarget) { LookTarget(); }

        if(isTriggerButton)
        {
            if(!input.RSB)
            {
                setIsTriggerButton(false);
            }
        }
    }
    protected virtual void ClearTarget() { }
    protected virtual void LockToTarget() { }
    protected virtual void LookTarget() { }
    public virtual void LookForward() { }
    private void setIsTriggerButton(bool value) { isTriggerButton = value; }
    protected void setIsLookTarget(bool value) { isLookTarget = value; }
    public void setIsMouse(bool value) { isMouse = value; }
}
