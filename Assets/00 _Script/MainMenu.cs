using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneReference baseScene;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnStartClick()
    {
        LoadSceneManager.Instance.LoadSceneSingle(baseScene.Name);
    }
}
