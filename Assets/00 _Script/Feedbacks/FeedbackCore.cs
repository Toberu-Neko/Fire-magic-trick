using System.Collections.Generic;
using UnityEngine;

public enum feebackType
{
    OnStart,
    OnEnd,
}
public class FeedbackCore : MonoBehaviour
{
    private readonly List<FeedbackCoreComponent> Feedbacks_OnStart = new();
    private readonly List<FeedbackCoreComponent> Feedbacks_OnEnd = new();

    #region For Other Script
    public void PlayFeedbacks()
    {
        foreach (FeedbackCoreComponent compent in Feedbacks_OnStart)
        {
            compent.PlayFeedback();
        }
    }
    public void StopFeedbacks()
    {
        foreach (FeedbackCoreComponent compent in Feedbacks_OnEnd)
        {
            compent.PlayFeedback();
        }
    }
    #endregion
    #region For Child Script
    public void AddCompent(FeedbackCoreComponent compent, feebackType type)
    {
        switch (type)
        {
            case feebackType.OnStart:
                AddCompent(compent, Feedbacks_OnStart, type);
                break;
            case feebackType.OnEnd:
                AddCompent(compent, Feedbacks_OnEnd, type);
                break;
        }
    }
    public void AddCompent(FeedbackCoreComponent compent, List<FeedbackCoreComponent> compents, feebackType type)
    {
        if (!compents.Contains(compent))
            compents.Add(compent);
    }
    #endregion
}
