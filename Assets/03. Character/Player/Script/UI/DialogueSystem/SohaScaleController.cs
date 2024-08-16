using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SohaScaleController : MonoBehaviour
{
    private bool close;

    private void Awake()
    {
        close = false;
    }

    void Update()
    {
        if(close)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 5);
            if (transform.localScale.x < 0.05f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Close()
    {
        close = true;
    }
}
