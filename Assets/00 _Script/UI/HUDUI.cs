using MoreMountains.Tools;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private Boss_UI bossUI;
    [SerializeField] private CrosshairUI crosshairUI;
    [SerializeField] private TeachFloat teachFloatUI;
    [field: SerializeField] public HUDVFX HudVFX { get; private set; }

    [SerializeField] private MMProgressBar normalBar;
    [SerializeField] private MMProgressBar overburnBar;

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
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
