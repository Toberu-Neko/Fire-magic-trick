using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Deactivate should always be called first before Activate any other UI.
/// </summary>
public class MainMenuUIBase : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectObj;
    public virtual void Activate()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectObj);
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
    }
}
