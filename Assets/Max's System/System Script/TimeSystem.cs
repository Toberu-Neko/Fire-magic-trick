using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// <para>�\��: �C���Ȱ�,�C���u���ĪG</para>
///<para> �Ϊk: �@�˩I�sTimeSystem.SetTimeScaleCall?.Invoke(�ɶ����v)</para>
///<para> �s�C���Ȱ� TimeSystem.PauseGameCall?.Invoke(�}��)</para>
///<para> �u���ĪG TimeSystem.PauseFrameCall?.Invoke(�ɪ�)</para>
///<para> �����ĪG TimeSystem.KeepPauseCall?.Invoke(�}��)</para>
///<para> �`�N�C���Ȱ�(�i�H�ճ]�w�h�X�C������)�M�u�����h�Ť��@��!�C���Ȱ��U�Ҧ��u���M�����L�ġA�C���Ȱ��n�F�O�o�n����~</para>
/// </summary>
public class TimeSystem : MonoBehaviour
{

    //�i�I�s�\��

    /// <summary>�]�w�ɶ����v(���v)</summary>
    public static Action<float> SetTimeScaleCall;
    /// <summary>�u��(��)</summary>
    public static Action<float> PauseFrameCall { get; private set; }
    /// <summary>����(�����~��) (�}��)</summary>
    public static Action<bool> KeepPauseCall;
    
    /// <summary>�ɶ��ܴ����i</summary>
    public static Action<float> TimeScaleChangeAnnounce;

    /// <summary>�C���Ȱ�(�}��)</summary>
    public static Action<bool> PauseGameCall;
    
    /// <summary>�C���Ȱ����i(�}��)</summary>
    public static Action<bool> GamePauseAnnounce;




    public static float CurrentTimeScale = 1;
    [HideInInspector] public bool isPauseGame;
    float LastTimeScale = 1;
    float LastPauseGameTimeScale = 1;
    (bool isPauseFramRequest, float targetEndPauseFarmeTime, float ReturnTimeScale) PauseFramLookUp;

    #region ���P��
    private void Awake()
    {
        SetTimeScaleCall += SetTimeScale;
        PauseFrameCall += PauseFrame;
        PauseGameCall += SetPauseGame;
        KeepPauseCall += KeepPause;
 

    }
    private void OnDestroy()
    {
        SetTimeScaleCall -= SetTimeScale;
        PauseGameCall -= SetPauseGame;
        KeepPauseCall -= KeepPause;

        Time.timeScale = 1;
    }
    public async void PauseFrame(float PauseTime)
    { 
        if (isPauseGame) { return; }
        PauseFramLookUp.isPauseFramRequest = true;
        PauseFramLookUp.targetEndPauseFarmeTime = Time.time + PauseTime;
        PauseFramLookUp.ReturnTimeScale = CurrentTimeScale;
        float RemainTimer = PauseTime;
        SetTimeScale(0);


        while (RemainTimer >= 0)
        {
            await System.Threading.Tasks.Task.Delay(100);

            if (isPauseGame)
            {
                return;
            }
            RemainTimer -= 0.1f;
        }
        PauseFramLookUp.isPauseFramRequest = false;
        ResetToLastTimeScale();

    }
    public void KeepPause(bool On)
    {
        if (isPauseGame) { return; }
        if (On)
        {
            SetTimeScale(0);
        }
        if (!On)
        {
            ResetToLastTimeScale();
        }
    }
    void SetTimeScale(float NewtimeScale)
    {
        LastTimeScale = Time.timeScale;
        Time.timeScale = NewtimeScale;
        CurrentTimeScale = NewtimeScale;
        TimeScaleChangeAnnounce?.Invoke(NewtimeScale);
    }
    void SetPauseGame(bool ispause)
    {
        isPauseGame = ispause;
        if (ispause)
        {
            LastPauseGameTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        if (!ispause)
        {

            Time.timeScale = LastPauseGameTimeScale == 0 ? 1 : LastPauseGameTimeScale;
        }
        GamePauseAnnounce?.Invoke(ispause);
    }
    void ResetToLastTimeScale()
    {
        if (LastTimeScale == 0)
        {       
            SetTimeScale(1);
        }
        else
        {
            SetTimeScale(LastTimeScale);
        }
    }
    public static bool isTimePause() { return CurrentTimeScale == 0; } 
    #endregion
}
