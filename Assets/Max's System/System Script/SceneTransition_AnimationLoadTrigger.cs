using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Animation�J���ʵe�]���O�o���ƥ�]�o��
/// </summary>
public class SceneTransition_AnimationLoadTrigger : MonoBehaviour
{
    public void AnimationFinishAnnounce()
    {
        SceneTransition.AnimationFinishLoadeSceneCall?.Invoke();
    }
}
