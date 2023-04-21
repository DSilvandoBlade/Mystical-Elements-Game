using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textPoints;
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private float m_maxSeconds;

    private int m_totalPoints;
    private Player m_player;

    private void Start()
    {
        DisplayPoints();

        m_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        m_maxSeconds -= Time.deltaTime;

        if (m_maxSeconds <= 0)
        {
            m_timerText.text = "TIMES OUT!";
            m_player.Death();
        }

        else
        {
            m_timerText.text = FormatTime(m_maxSeconds);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(m_maxSeconds / 60);
        int seconds = Mathf.FloorToInt(m_maxSeconds) - (minutes * 60);

        return minutes.ToString("0") + ":" + seconds.ToString("00");
    }

    private void DisplayPoints()
    {
        m_textPoints.text = m_totalPoints.ToString("00000");
    }

    public void AddPoints(int pointsToAdd)
    {
        m_totalPoints = Mathf.Clamp(m_totalPoints + pointsToAdd, 0, 99999);
        DisplayPoints();
    }
}
