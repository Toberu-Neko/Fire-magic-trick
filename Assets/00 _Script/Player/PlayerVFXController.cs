using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField] private GameObject superDashVFX;

    public void SetSuperDashVFX(bool value)
    {
        superDashVFX.SetActive(value);
    }

}
