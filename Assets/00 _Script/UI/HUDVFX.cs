using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitPlayerEffect;
    [SerializeField] private ParticleSystem hitPlayerSpeedLine;
    [SerializeField] private ParticleSystem fireAlt;
    [SerializeField] private ParticleSystem windAlt;

    [SerializeField] private GameObject inFireState;
    [SerializeField] private GameObject inWindState;
    [SerializeField] private GameObject overburn;
    [SerializeField] private GameObject windEnergyFull;
    [SerializeField] private GameObject fireEnergyFull;
    [SerializeField] private GameObject runSpeedLine;
    [SerializeField] private GameObject superDashSpeedLine;

    private void Awake()
    {
        overburn.SetActive(false);
        windEnergyFull.SetActive(false);
        fireEnergyFull.SetActive(false);
        runSpeedLine.SetActive(false);
        superDashSpeedLine.SetActive(false);
        inFireState.SetActive(false);
        inWindState.SetActive(false);
    }

    public void HitPlayerEffect()
    {
        hitPlayerEffect.Play();
    }

    public void FireAltEffect()
    {
        fireAlt.Play();
    }

    public void WindAltEffect()
    {
        windAlt.Play();
    }

    public void FireStateIndicater(bool value)
    {
        inFireState.SetActive(false);
        inFireState.SetActive(value);
        inWindState.SetActive(false);
    }

    public void WindStateIndicater(bool value)
    {
        inWindState.SetActive(false);
        inWindState.SetActive(value);
        inFireState.SetActive(false);
    }

    public void OverburnEffect(bool value)
    {
        overburn.SetActive(value);
    }

    public void WindEnergyFullEffect(bool value)
    {
        windEnergyFull.SetActive(value);
    }

    public void FireEnergyFullEffect(bool value)
    {
        fireEnergyFull.SetActive(value);
    }

    public void RunSpeedLineEffect(bool value)
    {
        runSpeedLine.SetActive(value);
    }

    public void SuperDashSpeedLineEffect(bool value)
    {
        superDashSpeedLine.SetActive(value);
    }
}
