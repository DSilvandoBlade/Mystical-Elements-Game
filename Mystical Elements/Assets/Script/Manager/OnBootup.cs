using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBootup : MonoBehaviour
{
    [SerializeField] private int m_targetFrameRate;
    [SerializeField] private int m_vSyncCount;

    [SerializeField] private float m_titlescreenSwitch;

    private void Start()
    {
        QualitySettings.vSyncCount = m_vSyncCount;
        Application.targetFrameRate = m_targetFrameRate;

        Invoke("GoToTitlescreen", m_titlescreenSwitch);
    }

    private void GoToTitlescreen()
    {
        SceneManager.LoadScene("Titlescreen");
    }
}
