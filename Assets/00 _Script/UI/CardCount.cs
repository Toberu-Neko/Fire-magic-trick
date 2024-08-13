using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CardCount : MonoBehaviour
{
    [SerializeField] private Slider fireSlider;
    [SerializeField] private Slider windSlider;

    private int targetFireCount;
    private int targetWindCount;

    private void Awake()
    {
        fireSlider.value = 0;
        windSlider.value = 0;
    }

    private void Update()
    {
        if (fireSlider.value != targetFireCount)
        {
            fireSlider.value = Mathf.Lerp(fireSlider.value, targetFireCount, 0.1f);
        }

        if (windSlider.value != targetWindCount)
        {
            windSlider.value = Mathf.Lerp(windSlider.value, targetWindCount, 0.1f);
        }
    }

    public void SetFireCount(int value)
    {
        targetFireCount = value;
    }




    public void SetWindCount(int value)
    {
        targetWindCount = value;
    }
}
