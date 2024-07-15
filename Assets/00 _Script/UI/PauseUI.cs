using Eflatun.SceneReference;
using UnityEngine;

public class PauseUI : MouseControlUIBase
{
    [SerializeField] private PauseUIMain pauseUIMain;
    [SerializeField] private OptionUI optionUI;


    public override void Activate()
    {
        base.Activate();

        GameManager.Instance.PauseGame();
        pauseUIMain.Activate();
        optionUI.gameObject.SetActive(false);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        GameManager.Instance.ResumeGame();
    }

}
