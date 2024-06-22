using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(MMF_Player))]
public class FeedbackCoreComponent : MonoBehaviour
{
    //Add this Feedback To FeedbackCore.

    public feebackType type;

    private MMF_Player feedback;
    private FeedbackCore feedbackCore;

    private void Awake()
    {
        feedback = GetComponent<MMF_Player>();
        feedbackCore = GetComponentInParent<FeedbackCore>();

        if(feedbackCore != null )
        {
            feedbackCore.AddCompent(this, type);
        }
    }
    public void PlayFeedback()
    {
        feedback.PlayFeedbacks();
    }
    public void StopFeedback()
    {
        feedback.StopFeedbacks();
    }
}
