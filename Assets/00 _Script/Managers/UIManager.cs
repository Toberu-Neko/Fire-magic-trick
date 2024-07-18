using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
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

    private void OnEnable()
    {
        pauseUI.gameObject.SetActive(false);
        deathUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.ESCInput)
        {
            PlayerInputHandler.Instance.UseESCInput();

            if (!pauseUI.gameObject.activeInHierarchy)
            {
                pauseUI.Activate();
            }
            else
            {
                pauseUI.Deactivate();
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
