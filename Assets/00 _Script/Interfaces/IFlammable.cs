using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlammable
{
    void SetOnFire(float duration);

    bool CheckIfOnFire();
}
