using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitlescreenManager : MonoBehaviour
{
    [SerializeField] private Button m_goButton;
    [SerializeField] private TextMeshProUGUI m_goButtonText;

    [SerializeField] private GameObject m_loadingScreen;
    [SerializeField] Image m_loadingBarFill;

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
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        m_loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            m_loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }

    public void GoToTutorialScene()
    {
        StartCoroutine(LoadScene("TutorialRoom"));
    }

    public void GoToSelectedScene()
    {
        if (m_sceneName == null)
        {
            return;
        }

        StartCoroutine(LoadScene(m_sceneName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
