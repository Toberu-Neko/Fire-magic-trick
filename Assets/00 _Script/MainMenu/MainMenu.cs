using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MainMenuUIBase
{
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [SerializeField] private OptionUI optionUI;

    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadButton;

    [SerializeField] private SceneReference baseScene;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;

        saveSlotMenu.gameObject.SetActive(false);
        optionUI.gameObject.SetActive(false);

        optionUI.OnDeactivate += OptionUI_OnDeactivate;
    }

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        optionUI.OnDeactivate -= OptionUI_OnDeactivate;
    }

    private void OptionUI_OnDeactivate()
    {
        Activate();
    }

    public void OnContinueClick()
    {
        LoadSceneManager.Instance.LoadSceneSingle(baseScene.Name);
    }

    public void OnStartClick()
    {
        Deactivate();

        saveSlotMenu.Activate(false);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnOptionClick()
    {
        Deactivate();

        optionUI.Activate();
    }

    public void OnCreditsClick()
    {

    }

    public void OnLoadClick()
    {
        saveSlotMenu.Activate(true);
    }

}
