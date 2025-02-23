using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour, IOptionData
{
    public event Action OnDeactivate;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider soundFXVolumeSlider;

    private int languageIndex;

    public void Activate()
    {
        DataPersistenceManager.Instance.LoadOptionData();
        EventSystem.current.SetSelectedGameObject(masterVolumeSlider.gameObject);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
        OnDeactivate?.Invoke();
        DataPersistenceManager.Instance.SaveOptionData();
        DataPersistenceManager.Instance.LoadOptionData();
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(Mathf.Log10(volume) * 20f);
    }

    public void SetSoundFXVolume(float volume)
    {
        AudioManager.Instance.SetSoundFXVolume(Mathf.Log10(volume) * 20f);
    }

    public void SetBGMVolume(float volume)
    {
        AudioManager.Instance.SetBGMVolume(Mathf.Log10(volume) * 20f);
    }

    public void ChangeLocaleButton(int index)
    {
        languageIndex = index;
        LocalizationManager.Instance.ChangeLocale(index);
    }


    public void LoadOptionData(OptionData data)
    {
        languageIndex = data.languageIndex;
        masterVolumeSlider.value = data.masterVolume;
        soundFXVolumeSlider.value = data.sfxVolume;
        bgmVolumeSlider.value = data.musicVolume;

        SetMasterVolume(data.masterVolume);
        SetSoundFXVolume(data.sfxVolume);
        SetBGMVolume(data.musicVolume);
        ChangeLocaleButton(data.languageIndex);
    }

    public void SaveOptionData(OptionData data)
    {
        data.languageIndex = languageIndex;
        data.masterVolume = masterVolumeSlider.value;
        data.sfxVolume = soundFXVolumeSlider.value;
        data.musicVolume = bgmVolumeSlider.value;
    }
}
