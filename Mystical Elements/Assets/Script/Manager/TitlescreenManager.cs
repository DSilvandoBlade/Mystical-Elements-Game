using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitlescreenManager : MonoBehaviour
{
    [SerializeField] private Button m_goButton;
    [SerializeField] private TextMeshProUGUI m_goButtonText;

    private string m_sceneName;

    public void GoButtonSet(string sceneName)
    {
        if (sceneName == null)
        {
            m_goButton.interactable = false;
            m_goButtonText.text = string.Empty;
            return;
        }

        m_goButton.interactable = true;
        m_goButtonText.text = "Go to " + sceneName;
        m_sceneName = sceneName;
    }

    public void GoToSelectedScene()
    {
        SceneManager.LoadScene(m_sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
