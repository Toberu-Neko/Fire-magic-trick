using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private DeathUI deathUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateDeathUI()
    {
        deathUI.Activate();
    }

    public void DeactivateDeathUI()
    {
        deathUI.Deactivate();
    }

    public bool IsDeathUIOpenFinished()
    {
        return deathUI.FinishedOpenAnim;
    }
}
