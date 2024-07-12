using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject floatVFX;
    [SerializeField] private GameObject windFeetCardVFX;
    [SerializeField] private GameObject superDashVFX;
    [SerializeField] private GameObject canComboVFX;

    [Header("Prefabs")]
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private GameObject respawnVFXPRefab;

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

        Init();
    }

    public void Init()
    {
        superDashVFX.SetActive(false);
        floatVFX.SetActive(false);
        windFeetCardVFX.SetActive(false);
        canComboVFX.SetActive(false);
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

    public void ActivateDeathVFX()
    {
        particleManager.StartParticles(deathVFXPrefab, transform.position);
    }

    public void ActivateRespawnVFX()
    {
        particleManager.StartParticles(respawnVFXPRefab, transform.position, transform);
    }

    public void SetCanComboVFX(bool value)
    {
        canComboVFX.SetActive(value);
    }

    public void SetFloatVFX(bool value)
    {
        floatVFX.SetActive(value);
    }

    public void SetWindFeetCardVFX(bool value)
    {
        windFeetCardVFX.SetActive(value);
    }

    public void SetSuperDashVFX(bool value)
    {
        superDashVFX.SetActive(value);
    }
}
