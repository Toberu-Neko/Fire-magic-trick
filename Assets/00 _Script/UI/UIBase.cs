using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
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
