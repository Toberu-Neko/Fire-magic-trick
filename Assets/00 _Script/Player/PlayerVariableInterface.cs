using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariableInterface : MonoBehaviour
{
    public Vector3 RespawnPosition { get; private set; }

    public void SetRespawnPosition(Vector3 position)
    {
        RespawnPosition = position;
    }
}
