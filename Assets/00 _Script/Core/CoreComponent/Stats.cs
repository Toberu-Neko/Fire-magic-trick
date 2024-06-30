using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    // 0 = Die, <100 = burn,  100 = Normal, 200 = NoSkill
    [field: SerializeField] public CoreStatSystem Health { get; private set; }

    private float healthRegenRate = 0.5f;
    // 0 = NoSkill, 100 = Normal, 100 < 200 = Burn, 200 = Die

    public bool IsBurning { get; private set; }

}
