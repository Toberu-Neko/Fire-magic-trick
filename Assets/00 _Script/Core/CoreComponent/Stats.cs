using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    // 0 = Die, < 100 = burn,  100 = Normal, 200 = NoSkill
    [field: SerializeField] public CoreStatSystem Health { get; private set; }

    public bool IsInvincible { get; private set; }
    public bool InCombat { get; private set; }
    [SerializeField] private float combatTimer = 5f;
    private float startCombatTime;

    [SerializeField] private float burnWhenHealthBelowPercentage = 0.5f;
    [Header("This is not for Player")]
    [SerializeField] private float healthRegenPerSec = 0f;
    public bool IsBurning { get; private set; }
    public event Action<bool> OnBurnChanged;
    private float startBurnTime;
    private float burnDuration;

    private void OnEnable()
    {
        IsBurning = false;
        InCombat = false;
        startBurnTime = 0f;
        burnDuration = 0f;
        startCombatTime = 0f;

        Health.Init();
        SetInvincible(false);

        Health.OnValueIncreased += SetInCombatTrue;
    }

    private void OnDisable()
    {
        Health.OnValueIncreased -= SetInCombatTrue;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (InCombat && Time.time >= startCombatTime + combatTimer)
        {
            InCombat = false;
        }

        if (!InCombat && Health.CurrentValue < Health.InitValue && healthRegenPerSec > 0f)
        {
            Health.IncreaseUntilInitValue(healthRegenPerSec * Time.deltaTime);
        }

        if(Health.CurrentValuePercentage < burnWhenHealthBelowPercentage)
        {
            SetIsBurning(true);
        }
        else if (IsBurning)
        {
            BurnCheck();
        }
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
            if(time > burnDuration)
            {
                burnDuration = time;
            }
        }

        SetIsBurning(true);
    }

    private void BurnCheck()
    {
        if (Time.time < startBurnTime + burnDuration)
        {
            return;
        }

        SetIsBurning(false);
    }

    private void SetIsBurning(bool value)
    {
        IsBurning = value;
        OnBurnChanged?.Invoke(value);
    }


    public void SetInvincible(bool value)
    {
        IsInvincible = value;
    }

    public void SetInCombatTrue()
    {
        startCombatTime = Time.time;
        InCombat = true;
    }
}
