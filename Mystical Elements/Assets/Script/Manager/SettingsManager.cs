using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider m_masterVolumeSlider;
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_UiVolumeSlider;
    [SerializeField] private Slider m_gameVolumeSlider;

    [SerializeField] private TMP_Dropdown m_GraphicsQualityDropdown;
    [SerializeField] private TMP_Dropdown m_languageDropdown;
    [SerializeField] private TMP_Dropdown m_antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown m_screenShakeDropdown;


    private void Start()
    {
        DataClass dataClass = JsonReadWriteSystem.LoadFromJson();

        m_masterVolumeSlider.value = dataClass.MasterVolume;
        m_musicVolumeSlider.value = dataClass.MusicVolume;
        m_UiVolumeSlider.value = dataClass.UiVolume;
        m_gameVolumeSlider.value = dataClass.GameVolume;
        m_GraphicsQualityDropdown.value = dataClass.GraphicsQuality;
        m_languageDropdown.value = dataClass.Language;
        m_antiAliasingDropdown.value = dataClass.AntiAliasing;
        m_screenShakeDropdown.value = dataClass.ScreenShake;
    }

    public void OnSettingsClose()
    {
        DataClass dataClass = new DataClass();

        dataClass.MasterVolume = m_masterVolumeSlider.value;
        dataClass.MusicVolume = m_musicVolumeSlider.value;
        dataClass.UiVolume = m_UiVolumeSlider.value;
        dataClass.GameVolume = m_gameVolumeSlider.value;
        dataClass.GraphicsQuality = m_GraphicsQualityDropdown.value;
        dataClass.Language = m_languageDropdown.value;
        dataClass.AntiAliasing = m_antiAliasingDropdown.value;
        dataClass.ScreenShake = m_screenShakeDropdown.value;

        JsonReadWriteSystem.SaveToJson(dataClass);
    }
}
