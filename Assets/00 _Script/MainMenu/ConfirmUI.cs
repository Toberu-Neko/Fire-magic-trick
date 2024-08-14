using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmUI : MainMenuUIBase
{
    [SerializeField] private MainMenu mainMenu;

    public void OnClickCancel()
    {
        Deactivate();

        mainMenu.Activate();
    }
}
