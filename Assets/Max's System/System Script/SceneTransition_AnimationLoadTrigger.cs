using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Animation入場動畫跑完記得插事件跑這個
/// </summary>
public class SceneTransition_AnimationLoadTrigger : MonoBehaviour
{
    public void AnimationFinishAnnounce()
    {
        SceneTransition.AnimationFinishLoadeSceneCall?.Invoke();
    }
}
