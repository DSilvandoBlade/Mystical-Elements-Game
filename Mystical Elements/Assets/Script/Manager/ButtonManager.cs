using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private string m_nextSceneName;

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Next()
    {
        SceneManager.LoadScene(m_nextSceneName);
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
