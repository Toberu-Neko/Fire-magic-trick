using Eflatun.SceneReference;
using UnityEngine;

public class PauseUI : MouseControlUIBase
{
    [SerializeField] private SceneReference mainMenuScene;
    public override void Activate()
    {
        base.Activate();

        GameManager.Instance.PauseGame();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        GameManager.Instance.ResumeGame();
    }

    public void OnResumeButton()
    {
        Deactivate();
    }

    public void OnMainMenuButton()
    {
        Deactivate();

        LoadSceneManager.Instance.LoadSceneSingle(mainMenuScene.Name);
    }
}
