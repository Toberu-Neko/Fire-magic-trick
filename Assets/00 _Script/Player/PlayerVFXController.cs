using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField] private GameObject superDashVFX;
    [SerializeField] private GameObject floatVFX;

    public void SetSuperDashVFX(bool value)
    {
        superDashVFX.SetActive(value);
    }

    public void SetFloatVFX(bool value)
    {
        floatVFX.SetActive(value);
    }

}
