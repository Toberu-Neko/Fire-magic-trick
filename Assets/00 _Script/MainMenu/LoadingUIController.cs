using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private HealthBar loadingBar;

    public void Start()
    {
        LoadSceneManager.Instance.LoadingObj = loadingObj;
        LoadSceneManager.Instance.OnLoadingSingleProgress += HandleLoadingSingleProgress;
        loadingBar.Init(1f);

        DataPersistenceManager.Instance.LoadOptionData();
    }

    private void OnDisable()
    {
        LoadSceneManager.Instance.OnLoadingSingleProgress -= HandleLoadingSingleProgress;
    }

    private void HandleLoadingSingleProgress(float progress)
    {
        loadingBar.UpdateHealthBar(1f - progress);
    }
}
