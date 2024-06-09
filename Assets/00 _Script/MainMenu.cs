using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneReference baseScene;
    public void OnStartClick()
    {
        LoadSceneManager.Instance.LoadSceneSingle(baseScene.Name);
    }
}
