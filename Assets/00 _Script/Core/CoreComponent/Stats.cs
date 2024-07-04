using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    // 0 = Die, < 100 = burn,  100 = Normal, 200 = NoSkill
    [field: SerializeField] public CoreStatSystem Health { get; private set; }

    private float healthRegenRate = 0.5f;

    public bool IsBurning { get; private set; }
    private float startBurnTime;
    private float burnDuration;

    private void OnEnable()
    {
        IsBurning = false;
        startBurnTime = 0f;
        burnDuration = 0f;

        Health.Init();
    }

    public void SetOnFire(float time)
    {
        if (!IsBurning)
        {
            startBurnTime = Time.time;
            burnDuration = time;
        }
        else
        {
            burnDuration += time;
        }

        IsBurning = true;
        StopCoroutine(BurnCheck());
        StartCoroutine(BurnCheck());
    }

    private IEnumerator BurnCheck()
    {
        while (Time.time < startBurnTime + burnDuration)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        IsBurning = false;
    }
}
