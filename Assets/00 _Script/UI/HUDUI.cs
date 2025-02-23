using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private Boss_UI bossUI;
    [SerializeField] private CrosshairUI crosshairUI;
    [SerializeField] private TeachFloat teachFloatUI;
    [field: SerializeField] public CardCount CardCount { get; private set; }
    [field: SerializeField] public HUDVFX HudVFX { get; private set; }

    [SerializeField] private MMProgressBar normalBar;
    [SerializeField] private Image normalBarColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private MMProgressBar overburnBar;
    [SerializeField] private Image overburnBarColor;
    [SerializeField] private Color overburnColor;

    private bool canChangeBar = true;

    private void Awake()
    {
        crosshairUI.SetCrossWhite();
        canChangeBar = true;
        teachFloatUI.gameObject.SetActive(false);
        bossUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        canChangeBar = true;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
    }

    public void SetCanChangeBar(bool value)
    {
        canChangeBar = value;
    }

    public void SetBar(float percentage)
    {
        if (!canChangeBar)
        {
            return;
        }

        if(percentage >= 0.5f)
        {
            normalBar.UpdateBar01(1f - (percentage - 0.5f) * 2f);
            overburnBar.UpdateBar01(0f);
        }
        else
        {
            normalBar.UpdateBar01(1f);
            overburnBar.UpdateBar01(1f - percentage * 2f);
        }
    }

    public void SetCrossRed()
    {
        crosshairUI.SetCrossRed();
    }

    public void SetCrossWhite()
    {
        crosshairUI.SetCrossWhite();
    }

    public void CrosshairShooting()
    {
        crosshairUI.CrosshairShooting();
    }

    public void HitEnemyEffect()
    {
        crosshairUI.HitEffextOn();
    }

    public void OpenTeachFloat(TeachFloat.types type)
    {
        teachFloatUI.Open(type);
    }

    public void CloseTeachFloat(TeachFloat.types type)
    {
        teachFloatUI.Close(type);
    }

    public void OpenBossUI(string boss_name, string littleTitle)
    {
        bossUI.Boss_Enter(boss_name, littleTitle);
    }

    public void CloseBossUI()
    {
        bossUI.Boss_Exit();
    }

    public void SetBossHealth(float value)
    {
        bossUI.SetValue(value);
    }



    public void Activate()
    {
        gameObject.SetActive(true);

        normalBarColor.color = normalColor;
        overburnBarColor.color = overburnColor;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
