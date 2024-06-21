using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadScene : MonoBehaviour
{
    [SerializeField] private BoxCollider col;
    [SerializeField] private SceneReference loadScene;
    
    private bool isLoaded = false;

    private void Awake()
    {
        isLoaded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLoaded)
            {
                LoadSceneManager.Instance.LoadSceneAdditive(loadScene.Name);
                isLoaded = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isLoaded)
            {
                LoadSceneManager.Instance.UnloadSceneAdditive(loadScene.Name);
                isLoaded = false;
            }
        }
    }
}
