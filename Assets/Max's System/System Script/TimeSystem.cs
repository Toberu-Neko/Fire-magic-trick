using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// <para>功用: 遊戲暫停,遊戲短停效果</para>
///<para> 用法: 一樣呼叫TimeSystem.SetTimeScaleCall?.Invoke(時間倍率)</para>
///<para> 叫遊戲暫停 TimeSystem.PauseGameCall?.Invoke(開關)</para>
///<para> 短停效果 TimeSystem.PauseFrameCall?.Invoke(時長)</para>
///<para> 持停效果 TimeSystem.KeepPauseCall?.Invoke(開關)</para>
///<para> 注意遊戲暫停(可以調設定退出遊戲那種)和短停的層級不一樣!遊戲暫停下所有短停和持停無效，遊戲暫停好了記得要關掉~</para>
/// </summary>
public class TimeSystem : MonoBehaviour
{

    //可呼叫功能

    /// <summary>設定時間倍率(倍率)</summary>
    public static Action<float> SetTimeScaleCall;
    /// <summary>短停(秒)</summary>
    public static Action<float> PauseFrameCall { get; private set; }
    /// <summary>長停(說關才關) (開關)</summary>
    public static Action<bool> KeepPauseCall;
    
    /// <summary>時間變換公告</summary>
    public static Action<float> TimeScaleChangeAnnounce;

    /// <summary>遊戲暫停(開關)</summary>
    public static Action<bool> PauseGameCall;
    
    /// <summary>遊戲暫停公告(開關)</summary>
    public static Action<bool> GamePauseAnnounce;




    public static float CurrentTimeScale = 1;
    [HideInInspector] public bool isPauseGame;
    float LastTimeScale = 1;
    float LastPauseGameTimeScale = 1;
    (bool isPauseFramRequest, float targetEndPauseFarmeTime, float ReturnTimeScale) PauseFramLookUp;

    #region 火星文
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
