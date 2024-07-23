using System;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private CrosshairUI crosshairUI;
    [SerializeField] private TeachFloat teachFloatUI;

    private void Awake()
    {
        crosshairUI.SetCrossWhite();
        teachFloatUI.gameObject.SetActive(false);
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

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
