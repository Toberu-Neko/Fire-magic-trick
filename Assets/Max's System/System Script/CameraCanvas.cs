using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自動調整 Canves 成正確尺寸
/// </summary>

public class CameraCanvas : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        if (GetComponent<CanvasScaler>() != null) { GetComponent<CanvasScaler>().referenceResolution = Camera.main.rect.size; }
    }
}
