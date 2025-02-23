using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    public GameObject LoadingObj { get; set; }
    public string CurrentSceneName { get; set; }

    public event Action<float> OnLoadingSingleProgress;
    public event Action<float> OnLoadingAdditiveProgress;
    public event Action<float> OnUnloadingAdditiveProgress;
    public event Action OnAdditiveSceneAlreadyLoaded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// This method is used to load a scene with a single scene.
    /// In this project, we use this method to load the main menu scene and ingame base scene.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneSingle(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncSingle(sceneName));
        CurrentSceneName = sceneName;
    }

    /// <summary>
    /// This method is used to load level scene ingame.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneAdditive(string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            OnAdditiveSceneAlreadyLoaded?.Invoke();
            Debug.LogWarning("Scene: " + sceneName + " is already loaded, this should only happened in Unity.");
            return;
        }
        StartCoroutine(LoadSceneAsyncAdditive(sceneName));
        CurrentSceneName = sceneName;
    }

    /// <summary>
    /// This method is used to unload the level scene.
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnloadSceneAdditive(string sceneName)
    {
        StartCoroutine(UnloadSceneAsuncAdditive(sceneName));
    }

    // For loading screen
    private IEnumerator LoadSceneAsyncSingle(string sceneName)
    {
        if(LoadingObj != null)
            LoadingObj.SetActive(true);
        ObjectPoolManager.ReturnAllObjectsToPool();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadingSingleProgress?.Invoke(progress); // UI_Manager.Instance.HandleLoadingSingleProgress();

            // Debug.Log("LoadSceneAsyncSingle " + progress);
            yield return null;
        }
    }

    // For confirming the level scene is loaded
    private IEnumerator LoadSceneAsyncAdditive(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadingAdditiveProgress?.Invoke(progress);
            // Debug.Log("LoadSceneAsyncAdditive " + progress);
            yield return null;
        }
    }


    // For unloading the level scene
    private IEnumerator UnloadSceneAsuncAdditive(string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName).isLoaded == false)
        {
            yield break;
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            float progress = Mathf.Clamp01(asyncUnload.progress / 0.9f);
            OnUnloadingAdditiveProgress?.Invoke(progress);
            // Debug.Log("UnloadSceneAsuncAdditive " + progress);
            yield return null;
        }

        // Debug.Log("UnloadSceneAsuncAdditive " + sceneName + " is done");
    }

}
