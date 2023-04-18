using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GradeSystem : MonoBehaviour
{
    [HideInInspector] public int TimesFellOff
    {
        get { return m_timesFellOff; }
        set { m_timesFellOff = value; }
    }

    [HideInInspector] public int EnemiesDefeated
    {
        get { return m_enemiesDefeated; }
        set { m_enemiesDefeated = value; }
    }

    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private Image m_gradeImage;
    [SerializeField] private TextMeshProUGUI m_gradeText;

    private int m_maxScore;

    private int m_timesFellOff;
    [SerializeField] private int m_timesFellScoreDecreasePerUnit;
    private int m_enemiesDefeated;
    [SerializeField] private int m_enemiesDefeatedScoreIncreasePerUnit;

    public Grade[] m_grades;

    public void CalculateAndDisplayGrade()
    {
        m_maxScore = (m_enemiesDefeated * m_enemiesDefeatedScoreIncreasePerUnit) - (m_timesFellOff * m_enemiesDefeatedScoreIncreasePerUnit);

        if (m_maxScore < 0)
        {
            m_maxScore = 0;
        }

        Grade chosenGrade = new Grade();

        foreach (Grade grade in m_grades)
        {
            if (grade.GradeScore >= m_maxScore)
            {
                chosenGrade = grade;
                break;
            }
        }
        
        m_scoreText.text = ("Score: ") + m_maxScore.ToString();

        m_gradeImage.sprite = chosenGrade.GradeSprite;
        m_gradeText.text = chosenGrade.GradeLetter;
    }
}

[System.Serializable]
public class Grade
{
    [SerializeField] public string GradeLetter;
    [SerializeField] public int GradeScore;
    [SerializeField] public Sprite GradeSprite;
}
