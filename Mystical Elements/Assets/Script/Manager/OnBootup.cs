using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBootup : MonoBehaviour
{
    [SerializeField] private int m_targetFrameRate;
    [SerializeField] private int m_vSyncCount;

    [SerializeField] private int m_masterVolume;
    [SerializeField] private int m_musicVolume;
    [SerializeField] private int m_uiVolume;
    [SerializeField] private int m_gameVolume;

    [SerializeField] private int m_antiAliasing;
    [SerializeField] private bool m_screenShake;

    [SerializeField] private float m_titlescreenSwitch;

    private void Start()
    {
        QualitySettings.vSyncCount = m_vSyncCount;
        Application.targetFrameRate = m_targetFrameRate;

        QualitySettings.antiAliasing = m_antiAliasing;

        Invoke("GoToTitlescreen", m_titlescreenSwitch);
    }
                                                                                                                                                                                                                                         
    private void GoToTitlescreen()
    {
        SceneManager.LoadScene("Titlescreen");
    }
}
