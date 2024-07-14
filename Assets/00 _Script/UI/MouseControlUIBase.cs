using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlUIBase : MonoBehaviour
{
    public virtual void Activate()
    {
        gameObject.SetActive(true);
        PlayerInputHandler.Instance.ResetAllInput();
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
