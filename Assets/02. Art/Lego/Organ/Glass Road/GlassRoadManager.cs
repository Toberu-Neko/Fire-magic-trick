using MoreMountains.Feedbacks;
using UnityEngine;

public class GlassRoadManager : MonoBehaviour
{
    private MMF_Player[] glassRoads;

    private void Awake()
    {
        glassRoads = GetComponentsInChildren<MMF_Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartAll();
        }
    }
    public void StartAll()
    {
        for(int i = 0; i < glassRoads.Length; i++)
        {
            glassRoads[i].PlayFeedbacks();
        }
    }
}
