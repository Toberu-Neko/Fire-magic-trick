using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderSFX : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayButtonHover(transform);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlayButtonHover(transform);
    }
}
