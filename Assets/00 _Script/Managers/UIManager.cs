using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private PauseUI pauseUI;
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

    private void Update()
    {
        if (PlayerInputHandler.Instance.ESCInput)
        {
            PlayerInputHandler.Instance.UseESCInput();
            if (pauseUI.gameObject.activeSelf)
            {
                pauseUI.Deactivate();
            }
            else
            {
                pauseUI.Activate();
            }
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

    public void ActivatePauseMenu()
    {
        pauseUI.Activate();
    }

    public void DeactivatePauseMenu()
    {
        pauseUI.Deactivate();
    }

    public bool IsDeathUIOpenFinished()
    {
        return deathUI.FinishedOpenAnim;
    }
}
