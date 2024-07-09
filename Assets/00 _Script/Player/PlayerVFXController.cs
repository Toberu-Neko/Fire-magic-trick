using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField] private GameObject floatVFX;
    [SerializeField] private GameObject windFeetCardVFX;
    [SerializeField] private GameObject superDashVFX;

    [Header("Super Jump")]
    [SerializeField] private Transform feetPosition;
    [SerializeField] private GameObject superJumpWindStartVFXPrefab;
    [SerializeField] private GameObject superJumpFireStartVFXPrefab;
    [SerializeField] private GameObject superJumpLandWindVFXPrefab;
    [SerializeField] private GameObject superJumpLandFireVFXPrefab;

    private Core core;
    private ParticleManager particleManager;
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        particleManager = core.GetCoreComponent<ParticleManager>();

        superDashVFX.SetActive(false);
        floatVFX.SetActive(false);
        windFeetCardVFX.SetActive(false);
    }


    public void SetSuperDashVFX(bool value)
    {
        superDashVFX.SetActive(value);
    }

    public void ActivateWindStartVFX()
    {
        particleManager.StartParticles(superJumpWindStartVFXPrefab, feetPosition.position);
    }

    public void ActivateFireStartVFX()
    {
        particleManager.StartParticles(superJumpFireStartVFXPrefab, feetPosition.position);
    }

    public void ActivateWindLandVFX()
    {
        particleManager.StartParticles(superJumpLandWindVFXPrefab, feetPosition.position);
    }

    public void ActivateFireLandVFX()
    {
        particleManager.StartParticles(superJumpLandFireVFXPrefab, feetPosition.position);
    }

    public void SetFloatVFX(bool value)
    {
        floatVFX.SetActive(value);
    }

    public void SetWindFeetCardVFX(bool value)
    {
        windFeetCardVFX.SetActive(value);
    }

}
