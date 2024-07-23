using System;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField] private HUDUI hudUI;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private DeathUI deathUI;

    [SerializeField] private TeachUI teachUI;
    public event Action OnTeachEnd;

    [SerializeField] private DialogueUI dialogueUI;
    public event Action OnDisplayNextSentence;
    public event Action OnDialogueEnd;

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
            else if (teachUI.gameObject.activeInHierarchy)
            {
                teachUI.CloseTeach();
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

    public void ActivateTeachUI(int index)
    {
        teachUI.OpenTeach(index);
        DeactivateHUD();
    }

    public void OnDecativateTeachUI()
    {
        OnTeachEnd?.Invoke();
        ActivateHUD();
    }

    #region HUD
    public void SetCrossRed()
    {
        hudUI.SetCrossRed();
    }

    public void SetCrossWhite()
    {
        hudUI.SetCrossWhite();
    }

    public void CrosshairShooting()
    {
        hudUI.CrosshairShooting();
    }

    public void HitEnemyEffect()
    {
        hudUI.HitEnemyEffect();
    }

    public void ActivateHUD()
    {
        hudUI.Activate();
    }

    public void DeactivateHUD()
    {
        hudUI.Deactivate();
    }

    public void OpenTeachFloat(TeachFloat.types type)
    {
        hudUI.OpenTeachFloat(type);
    }

    public void CloseTeachFloat(TeachFloat.types type)
    {
        hudUI.CloseTeachFloat(type);
    }
    #endregion

    #region Dialogue
    public void StartDialogue(SO_Dialogue dialogue)
    {
        dialogueUI.StartDialogue(dialogue);
    }
    public void StartDialogue(SO_Dialogue dialogue, float time)
    {
        dialogueUI.StartDialogue(dialogue, time);
    }

    public void DialogueEnd()
    {
        OnDialogueEnd?.Invoke();
    }

    public void DisplayNextSentence()
    {
        OnDisplayNextSentence?.Invoke();
    }
    #endregion

    public bool IsDeathUIOpenFinished()
    {
        return deathUI.FinishedOpenAnim;
    }
}
