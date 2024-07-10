using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using Cinemachine.PostFX;
using System;


/// <summary>
/// 需要用到Cinemachine! 請下載好~
///<para> 功用: 鏡頭搖晃，切換 Post Process(Volume)，鏡頭跟隨和切換(使用 Cinemachine 的 Camera State Machine)</para>
///<para> 用法: 搖晃的話和聲音一樣 new 一個 ScreenShakePack 叫 Play() 就好</para>
///<para> 換Post Process 叫 CCamScript.ChangeVolumeCall?.Invoke(新Volume);</para>
///<para> 切鏡頭和跟隨還是請要先叫參考喔! 直接叫 CCScript.Current 就可以使用內部程式了</para>
///<para> 有跟蹤鏡頭的話請把跟蹤目標設為本遊戲物件底下的TargetFocesPoint</para>
/// </summary>
public class CCamScript : MonoBehaviour
{
    public CinemachineImpulseSource ImpulseSource;
    CinemachineVolumeSettings VolumeSetting;
    VolumeProfile DefaultVolume;

    SignalSourceAsset DefultShakeNoise;

    [Header("Camera State Machine")]
    public Animator StateCameraAnim;

    public Transform TargetFocesPoint;
    public static CCamScript Current { get { return FindObjectOfType<CCamScript>(true); } }



    //可呼叫功能
    /// <summary> 鏡頭搖晃，建議直接在Pack叫就可以了 </summary>
    public static Action<ScreenShakePack> ScreenShakeCall;
    /// <summary> 中止鏡頭搖晃</summary>
    public static Action StopScreenShakeCall;
    /// <summary> 換Post Process</summary>
    public static Action<VolumeProfile> ChangeVolumeCall;
    /// <summary> 重置Post Process</summary>
    public static Action ResetVolumeCall;


    // 鏡頭功能 (使用State Cam)
    /// <summary>
    /// 切換另一個鏡頭
    /// </summary>
    /// <param name="Name">State Cam Animator 的 trigger 名稱</param>
    public void SetCameraAnim(string Name)
    {
        if (StateCameraAnim != null) { StateCameraAnim.Play(Name); }
    }


    /// <summary>
    /// 指定鏡頭看的點
    /// </summary>
    /// <param name="Point">跟蹤位置</param>
    public void SetFocesPoint(Vector2 Point)
    {
        TargetFocesPoint.position = Point;
    }


    /// <summary>
    /// 將Foces Point依附在某物件底下
    /// </summary>
    /// <param name="transform">跟蹤目標</param>
    public void SetFocesTransform(Transform transform)
    {
        TargetFocesPoint.transform.parent = transform;
        TargetFocesPoint.transform.localPosition = Vector3.zero;
    }



    #region 火星文 

    void Awake()
    {
        ScreenShakeCall += ScreenShake;
        ChangeVolumeCall += ChangeVolume;
        ResetVolumeCall += ResetVolume;
        StopScreenShakeCall += StopScreenShake;
        DefultShakeNoise = ImpulseSource.m_ImpulseDefinition.m_RawSignal;
        VolumeSetting = FindObjectOfType<CinemachineVolumeSettings>(true);

        if (FindObjectOfType<CinemachineStateDrivenCamera>() != null) { StateCameraAnim = FindObjectOfType<CinemachineStateDrivenCamera>().gameObject.GetComponent<Animator>(); }
        if (VolumeSetting != null) { DefaultVolume = VolumeSetting.m_Profile; }

    }
    private void OnDestroy()
    {
        ScreenShakeCall -= ScreenShake;
        ChangeVolumeCall -= ChangeVolume;
        ResetVolumeCall -= ResetVolume;
        StopScreenShakeCall -= StopScreenShake;
        ResetVolume();

    }

    public void ScreenShake(ScreenShakePack screenShakePack)
    {
        if (ImpulseSource == null) { return; }
        ImpulseSource.m_ImpulseDefinition.m_RawSignal = screenShakePack.OverrideShakeNoise == null ? DefultShakeNoise : screenShakePack.OverrideShakeNoise;
        ImpulseSource.m_ImpulseDefinition.m_AmplitudeGain = screenShakePack.AmplitudeGain;
        ImpulseSource.m_ImpulseDefinition.m_FrequencyGain = screenShakePack.FrequencyGain;
        ImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = screenShakePack.AttackTime;
        ImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = screenShakePack.StayTime;
        ImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = screenShakePack.DecayTime;

        ImpulseSource.GenerateImpulse();
    }

    void StopScreenShake()
    {
        ScreenShake(new(0, 0, 0, 0));
    }

    public void ChangeVolume(VolumeProfile NewVolume)
    {
        if (VolumeSetting == null) { return; }
        VolumeSetting.m_Profile = NewVolume;
    }
    public void ResetVolume()
    {
        if (VolumeSetting == null) { return; }
        VolumeSetting.m_Profile = DefaultVolume;
    }
    #endregion
}

//鏡頭搖晃包?和聲音包一樣
[System.Serializable]
public class ScreenShakePack
{
    public float AmplitudeGain = 1;
    public float AttackTime = 0;
    public float StayTime = 0.1f;
    public float FrequencyGain = 1;
    public float DecayTime = 0.5f;
    public SignalSourceAsset OverrideShakeNoise;
    public ScreenShakePack(float amplitudeGain, float stayTime, float frequencyGain, float decayTime)
    {
        AmplitudeGain = amplitudeGain;
        StayTime = stayTime;
        FrequencyGain = frequencyGain;
        DecayTime = decayTime;
    }
    public void Paly()
    {
        CCamScript.ScreenShakeCall?.Invoke(this);
    }
}