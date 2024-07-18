using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerHandler
{
    void GotoCantControlState();
    void GotoAfterSuperDashJumpState();
    void FinishCantControlState();
    void SetModel(bool value);
    void Teleport(Vector3 position);
}
