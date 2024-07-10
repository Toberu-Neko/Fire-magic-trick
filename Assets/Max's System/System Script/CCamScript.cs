using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using Cinemachine.PostFX;
using System;


/// <summary>
/// �ݭn�Ψ�Cinemachine! �ФU���n~
///<para> �\��: ���Y�n�̡A���� Post Process(Volume)�A���Y���H�M����(�ϥ� Cinemachine �� Camera State Machine)</para>
///<para> �Ϊk: �n�̪��ܩM�n���@�� new �@�� ScreenShakePack �s Play() �N�n</para>
///<para> ��Post Process �s CCamScript.ChangeVolumeCall?.Invoke(�sVolume);</para>
///<para> �����Y�M���H�٬O�Эn���s�Ѧҳ�! �����s CCScript.Current �N�i�H�ϥΤ����{���F</para>
///<para> ���������Y���ܽЧ���ܥؼг]�����C�����󩳤U��TargetFocesPoint</para>
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



    //�i�I�s�\��
    /// <summary> ���Y�n�̡A��ĳ�����bPack�s�N�i�H�F </summary>
    public static Action<ScreenShakePack> ScreenShakeCall;
    /// <summary> �������Y�n��</summary>
    public static Action StopScreenShakeCall;
    /// <summary> ��Post Process</summary>
    public static Action<VolumeProfile> ChangeVolumeCall;
    /// <summary> ���mPost Process</summary>
    public static Action ResetVolumeCall;


    // ���Y�\�� (�ϥ�State Cam)
    /// <summary>
    /// �����t�@�����Y
    /// </summary>
    /// <param name="Name">State Cam Animator �� trigger �W��</param>
    public void SetCameraAnim(string Name)
    {
        if (StateCameraAnim != null) { StateCameraAnim.Play(Name); }
    }


    /// <summary>
    /// ���w���Y�ݪ��I
    /// </summary>
    /// <param name="Point">���ܦ�m</param>
    public void SetFocesPoint(Vector2 Point)
    {
        TargetFocesPoint.position = Point;
    }


    /// <summary>
    /// �NFoces Point�̪��b�Y���󩳤U
    /// </summary>
    /// <param name="transform">���ܥؼ�</param>
    public void SetFocesTransform(Transform transform)
    {
        TargetFocesPoint.transform.parent = transform;
        TargetFocesPoint.transform.localPosition = Vector3.zero;
    }



    #region ���P�� 

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

//���Y�n�̥]?�M�n���]�@��
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