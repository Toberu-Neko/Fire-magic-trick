using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private Animator CrosshairAnimator;
    [SerializeField] private Color Normal;
    [SerializeField] private Color Check;
    [SerializeField] private Image[] CrosshairImages;
    [Header("Hit")]
    [SerializeField] private MMF_Player hitNear;
    [SerializeField] private MMF_Player hitFar;

    private void Awake()
    {
        SetCrossWhite();
    }

    public void SetCrossRed()
    {
        foreach (var material in CrosshairImages)
        {
            if(material.color == Check)
            {
                return;
            }
            else
            {
                material.color = Check;
            }
        }
    }

    public void SetCrossWhite()
    {
        foreach (var material in CrosshairImages)
        {
            if(material.color == Normal)
            {
                return;
            }
            else
            {
                material.color = Normal;
            }
        }
    }

    public void CrosshairShooting()
    {
        CrosshairAnimator.Play("Crosshair");
    }

    public void CrosshairHit()
    {
        CrosshairAnimator.Play("CrossHairHit");
    }

    public void EnemyHitImpluse(Vector3 hitPosition)
    {
        /*
        float distance = Vector3.Distance(hitPosition, transform.position);
        if(distance < thresholdDistance)
        {
            hitNear.PlayFeedbacks();
        }else
        {
              hitFar.PlayFeedbacks();
        }
        */
    }
}
