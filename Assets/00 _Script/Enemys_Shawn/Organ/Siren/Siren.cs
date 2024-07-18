using System.Threading.Tasks;
using UnityEngine;

public class Siren : MonoBehaviour
{
    private FeedbackCore feedbackCore;

    private void Awake()
    {
        feedbackCore = GetComponent<FeedbackCore>();
    }
    public async void Play()
    {
        feedbackCore.PlayFeedbacks();
        await Task.Delay(500);
        feedbackCore.StopFeedbacks();
    }
}
