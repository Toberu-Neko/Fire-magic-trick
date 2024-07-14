using UnityEngine;

public class TimelineState : MonoBehaviour
{
    // I dont know why Unity haven't timeline OnComplete function or event. by Shawn.

    [HideInInspector] public bool isCompelete = true;

    private bool isPlay;

    public void OnStart() 
    {
        if(!isPlay)
        {
            isCompelete = false;
            isPlay = true;
            Debug.Log("need:When timeline start, Player Can't control and rotateCamera.");
        }
    }

    public void OnComplete() 
    {
        isCompelete = true;
        isPlay = false;
        Debug.Log("need:When Timeline end, Player Can't control and rotateCamera.");
    }
}
