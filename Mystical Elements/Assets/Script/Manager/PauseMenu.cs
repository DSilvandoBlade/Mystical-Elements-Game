using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseCanvas;

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
}
