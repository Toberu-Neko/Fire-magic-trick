using Eflatun.SceneReference;
using UnityEngine;

public class PauseUIMain : MouseControlUIBase
{
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private OptionUI optionUI;

    private void Awake()
    {
        optionUI.OnDeactivate += OptionUI_OnDeactivate;
    }

    private void OnDestroy()
    {
        optionUI.OnDeactivate -= OptionUI_OnDeactivate;
    }

    private void OptionUI_OnDeactivate()
    {
        Activate();
    }

    public void OnResumeButton()
    {
        pauseUI.Deactivate();
    }

    public void OnMainMenuButton()
    {
        GameManager.Instance.ResumeGame();
        Time.timeScale = 1;
        DataPersistenceManager.Instance.SaveGame();
        LoadSceneManager.Instance.LoadSceneSingle(mainMenuScene.Name);
    }

    public void OnOptionButton()
    {
        Deactivate();

        optionUI.Activate();
    }
}
