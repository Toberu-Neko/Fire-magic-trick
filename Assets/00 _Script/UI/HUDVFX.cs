using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitPlayerEffect;
    [SerializeField] private ParticleSystem superDashHitSpeedLine;
    [SerializeField] private ParticleSystem fireAlt;
    [SerializeField] private ParticleSystem windAlt;
    [SerializeField] private Animator flipCardAnim;

    [SerializeField] private GameObject inFireState;
    [SerializeField] private GameObject inWindState;
    [SerializeField] private GameObject overburn;
    [SerializeField] private GameObject runSpeedLine;
    [SerializeField] private GameObject superDashSpeedLine;

    private void Awake()
    {
        hitPlayerEffect.gameObject.SetActive(true);
        superDashHitSpeedLine.gameObject.SetActive(true);
        fireAlt.gameObject.SetActive(true);
        windAlt.gameObject.SetActive(true);

        overburn.SetActive(false);
        runSpeedLine.SetActive(false);
        superDashSpeedLine.SetActive(false);
        inFireState.SetActive(false);
        inWindState.SetActive(false);
    }

    private void Start()
    {
        FlipToFireCard();
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

    public void SuperDashHitSpeedLineEffect()
    {
        superDashHitSpeedLine.Play();
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

    public void RunSpeedLineEffect(bool value)
    {
        runSpeedLine.SetActive(value);
    }

    public void SuperDashSpeedLineEffect(bool value)
    {
        superDashSpeedLine.SetActive(value);
    }

    public void FlipToFireCard()
    {
        flipCardAnim.SetBool("isFire", true);
        flipCardAnim.SetBool("isWind", false);
    }

    public void FlipToWindCard()
    {
        flipCardAnim.SetBool("isFire", false);
        flipCardAnim.SetBool("isWind", true);
    }
}
