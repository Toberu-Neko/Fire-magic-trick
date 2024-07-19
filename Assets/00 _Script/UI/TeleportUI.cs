using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUI : MouseControlUIBase
{
    [SerializeField] private Transform[] teleportPoint;

    public void OnClickTeleportButton(Transform transform)
    {
        GameManager.Instance.Player.transform.position = transform.position;
    }
}
