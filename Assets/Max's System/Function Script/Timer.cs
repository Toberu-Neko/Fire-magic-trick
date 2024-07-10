using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


/// <summary>
/// <para>��IE��n�Χ�K�y���D�P�B���� (��ı�o��)</para> 
/// <para> new(�]�ɶ�) �@�ӥX�ӡA�j�@�ǵ{���� UnityEvent �� ,Play() �� Restart() �N�i�H���o! </para> 
/// �ϥ�async
/// </summary>
public class Timer
{
    public Timer(float _Time = 0, float _CheckRate = 0.1f)
    {
        Time = _Time;
        CheckInTime = 0;
        TimesUpEvent = new();
        StartTimingEvent = new();
        TickEvent = new();
        DefalutSetTime = Time;
        Cancellation = new();
        CheckRate = Mathf.RoundToInt(_CheckRate * 1000);
        IsTiming = false;

    }
    public Timer(float _Time = 0)
    {
        Time = _Time;
        CheckInTime = 0;

        TimesUpEvent = new();
        StartTimingEvent = new();
        TickEvent = new();

        DefalutSetTime = Time;
        Cancellation = new();
        CheckRate = Mathf.RoundToInt(_Time * 0.9f * 1000);
        IsTiming = false;

    }
    float Time;

    // for only the newest timer counts
    int TimeID;
    readonly float DefalutSetTime;
    float CheckInTime;
    int CheckRate;

    public bool IsTiming;
    readonly public UnityEngine.Events.UnityEvent TimesUpEvent = new();
    readonly public UnityEngine.Events.UnityEvent StartTimingEvent = new();
    readonly public UnityEngine.Events.UnityEvent TickEvent = new();

    System.Threading.CancellationTokenSource Cancellation;

    public void Start()
    {
        CheckInTime = UnityEngine.Time.time;
        Cancellation = new();

        TimeID++;
        IsTiming = true;
        StartTimingEvent?.Invoke();
        TimerLook(TimeID);
    }
    public void SubtarctTime(float Secend)
    {
        if (GetTimer() - Secend <= 0)
        {
            CheckInTime -= GetTimer() + 0.1f;
        }
        else
        {
            CheckInTime -= Secend;
        }



        if (IsTiming)
        {
            if (GetTimer() <= 0)
            {
                Cancellation.Cancel();

                TimesUpEvent.Invoke();
                IsTiming = false;
            }
        }
    }
    public void Start(float _Time)
    {
        CheckInTime = UnityEngine.Time.time;
        Cancellation = new();
        TimeID++;
        Time = _Time;
        IsTiming = true;
        StartTimingEvent?.Invoke();
        TimerLook(TimeID);
    }
    public void Pause()
    {
        Time -= (UnityEngine.Time.time - CheckInTime);
        IsTiming = false;
        Cancellation.Cancel();
    }
    public void Stop()
    {
        Time = DefalutSetTime;
        IsTiming = false;
        TimeID++;
        Cancellation.Cancel();
    }
    public void Restart()
    {
        Stop();
        IsTiming = true;
        Start();

    }
    public void RestartDefultTime()
    {
        Stop();
        IsTiming = true;
        Start(DefalutSetTime);
    }

    async void TimerLook(int CheckID)
    {
        int Check = GetTimer() < CheckRate / 1000f ? Mathf.CeilToInt(GetTimer() * 1000f) : CheckRate;
        await Task.Delay(Mathf.Clamp(Check, 2, Check));
        if (TimeID != CheckID) { return; }

        FrameCheck(CheckID);
    }
    void FrameCheck(int CheckID)
    {

        if (UnityEngine.Time.time - CheckInTime > Time)
        {
            CheckInTime = UnityEngine.Time.time;
            TimesUpEvent.Invoke();
            IsTiming = false;
        }
        else
        {
            TickEvent?.Invoke();
            TimerLook(CheckID);
        }
    }

  
    /// <returns> �Ѿl�ɶ�</returns>
    public float GetTimer() { return Time - (UnityEngine.Time.time - CheckInTime); }


    /// <returns> �w�}�l�p�ɮɪ�</returns>
    public float GetInverseTimer() { return UnityEngine.Time.time - CheckInTime; }

    /// <summary>�]�w�p�ɮɶ� </summary>
    public void SetTimer(float _Time) { Time = _Time; }

    /// <returns> �]�w�ɶ�</returns>
    public float GetSetTime() { return Time; }

    public void SetCheckRate(float _CheckRate) { CheckRate = Mathf.RoundToInt(_CheckRate * 1000); }
    public void SetCheckRate() { CheckRate = Mathf.RoundToInt(Time * 1000) - 20; }
}
