using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseCanvas;
    [SerializeField] private GameObject m_exitWarning;

    public void Pause()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1;
    }

    public void OpenWarning()
    {
        m_exitWarning.SetActive(true);
    }

    public void CloseWarning()
    {
        m_exitWarning.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Resume();
        SceneManager.LoadScene("Titlescreen");
    }
}
