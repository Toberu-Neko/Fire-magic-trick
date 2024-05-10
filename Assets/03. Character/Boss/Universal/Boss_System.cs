using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public class Boss_System : MonoBehaviour
{
    [Header("Setting")]
    public Boss_UI UI;
    public Barrier barrier;
    [SerializeField] private MMF_Player reserFeedback;
    [Header("Boss")]
    [SerializeField] private string boss_name;
    [SerializeField] private string boss_littleTitle;
    [Space(10)]

    private ProgressSystem progress;

    public delegate void OnStartFightHandler();
    public event OnStartFightHandler onStartFight;
    public delegate void OnResetFightHandler();
    public event OnResetFightHandler onResetFight;
    public delegate void OnEndFightHandler();
    public event OnEndFightHandler onEndFight;

    private bool isBoss;
    private bool isWin;

    private void Start()
    {
        progress = GameManager.singleton.GetComponent<ProgressSystem>();

        progress.OnPlayerDeath += ResetBoss;
    }
    private void Update()
    {
        if(isBoss && !isWin)
        {
            if(this.gameObject.activeSelf==false)
            {
                this.gameObject.SetActive(true);
            }
        }
    }
    public void ResetBoss()
    {
        if (isWin) return;
        if (isBoss)
        {
            isBoss = false;

            UI.Boss_Exit();
            barrier.Close();
            reserFeedback.PlayFeedbacks();
            onResetFight?.Invoke();
            Debug.Log("Boss Fight Reset");
        }
    }
    public void StartBossFight()
    {
        if (isWin) return;
        if (!isBoss)
        {
            isBoss = true;

            UI.Boss_Enter(boss_name, boss_littleTitle);
            barrier.Open();
            reserFeedback.PlayFeedbacks();
            onStartFight?.Invoke();
        }
    }
    public void EndBossFight()
    {
        if(isBoss)
        {
            isBoss = false;

            onEndFight?.Invoke();
            UI.Boss_Exit();
            barrier.Close();
        }
    }
    public void SetHealth(float newHealthpersen)
    {
        UI.SetValue(newHealthpersen);
    }
    public void SetIsWind(bool active)
    {
        isWin = active;
    }
    public void DebugTest(string word)
    {
        Debug.Log(word);
    }
}
