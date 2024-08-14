using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public class Boss_System : DataPersistMapObjBase
{
    [Header("Setting")]
    public Barrier barrier;
    [SerializeField] private MMF_Player reserFeedback;
    [Header("Boss")]
    [SerializeField] private string boss_name;
    [SerializeField] private string boss_littleTitle;
    [SerializeField] private string bossBgmName;
    [SerializeField] private string normalBgmName;

    public delegate void OnStartFightHandler();
    public event OnStartFightHandler onStartFight;
    public delegate void OnResetFightHandler();
    public event OnResetFightHandler onResetFight;
    public delegate void OnEndFightHandler();
    public event OnEndFightHandler onEndFight;

    private bool isBoss;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnPlayerReborn += ResetBoss;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerReborn -= ResetBoss;
    }
    private void Update()
    {
        if(isBoss && !isActivated)
        {
            if (this.gameObject.activeSelf == false) 
            {
                this.gameObject.SetActive(true);
            }
        }
    }
    public void ResetBoss()
    {
        if (isActivated) return;
        if (isBoss)
        {
            isBoss = false;

            UIManager.Instance.HudUI.CloseBossUI();
            barrier.Close();
            reserFeedback.PlayFeedbacks();
            onResetFight?.Invoke();
            Debug.Log("Boss Fight Reset");
        }
    }
    public void StartBossFight()
    {
        if (isActivated) return;
        if (!isBoss)
        {
            isBoss = true;

            Debug.Log("Boss Fight Start");
            UIManager.Instance.HudUI.OpenBossUI(boss_name, boss_littleTitle);
            barrier.Open();
            reserFeedback.PlayFeedbacks();
            onStartFight?.Invoke();

            AudioManager.Instance.PlayBGM(bossBgmName);
        }
    }
    public void EndBossFight()
    {
        if(isBoss)
        {
            isBoss = false;

            onEndFight?.Invoke();
            AudioManager.Instance.PlayBGM(normalBgmName);
            UIManager.Instance.HudUI.CloseBossUI();
            barrier.Close();
        }
    }
    public void SetHealth(float newHealthpersen)
    {
        UIManager.Instance.HudUI.SetBossHealth(newHealthpersen);
    }
    public void SetIsWind(bool active)
    {
        isActivated = active;
    }
    public void DebugTest(string word)
    {
        Debug.Log(word);
    }
}
