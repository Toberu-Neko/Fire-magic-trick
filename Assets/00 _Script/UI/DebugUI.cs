using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI windText;
    [SerializeField] private TextMeshProUGUI fireText;

    [SerializeField] private Stats playerStats;
    [SerializeField] private CardSystem cardSystem;

    private void OnEnable()
    {
        playerStats.Health.OnValueChanged += Health_OnValueChanged;
        cardSystem.OnWindCardEnergyChanged += OnSetWindText;
        cardSystem.OnFireCardEnergyChanged += OnSetFireText;

        Health_OnValueChanged();
        OnSetWindText(0);
        OnSetFireText(0);
    }

    private void OnDisable()
    {
        playerStats.Health.OnValueChanged -= Health_OnValueChanged;
        cardSystem.OnWindCardEnergyChanged -= OnSetWindText;
        cardSystem.OnFireCardEnergyChanged -= OnSetFireText;
    }

    private void Health_OnValueChanged()
    {
        healthText.text = "Health: " + playerStats.Health.CurrentValue;
    }

    private void OnSetWindText(int value)
    {
        windText.text = "Wind Energy: " + value;
    }

    private void OnSetFireText(int value)
    {
        fireText.text = "Fire Energy: " + value;
    }
}
