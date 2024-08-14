using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning("Button component not found");
            return;
        }

        button.onClick.AddListener(HandleOnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button == null)
        {
            return;
        }

        if (!button.interactable)
        {
            return;
        }
        AudioManager.Instance.PlayButtonHover(transform);
    }

    private void HandleOnClick()
    {
       AudioManager.Instance.PlayButtonClick(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button == null)
        {
            return;
        }

        if (!button.interactable)
        {
            Debug.LogWarning("Button is not interactable");
            return;
        }

        AudioManager.Instance.PlayButtonClick(transform);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (button == null)
        {
            return;
        }

        if (!button.interactable)
        {
            return;
        }
        AudioManager.Instance.PlayButtonHover(transform);
    }
}
