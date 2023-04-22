using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GradeSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private Image m_gradeImage;
    [SerializeField] private TextMeshProUGUI m_gradeCommentText;
    [SerializeField] private TextMeshProUGUI m_gradeText;

    private int m_maxScore;
    private LevelManager m_manager;

    public Grade[] m_grades;

    private void Start()
    {
        m_manager = FindObjectOfType<LevelManager>();
    }

    public void CalculateAndDisplayGrade()
    {
        m_maxScore = m_manager.TotalPoints;

        Grade chosenGrade = new Grade();

        foreach (Grade grade in m_grades)
        {
            if (grade.GradeScore <= m_maxScore)
            {
                chosenGrade = grade;
                break;
            }
        }
        
        m_scoreText.text = ("Score: ") + m_maxScore.ToString();

        m_gradeImage.sprite = chosenGrade.GradeSprite;
        m_gradeText.text = chosenGrade.TheGrade;
        m_gradeCommentText.text = chosenGrade.GradeComment;
        
    }
}

[System.Serializable]
public class Grade
{
    [SerializeField] public string TheGrade;
    [SerializeField] public string GradeComment;
    [SerializeField] public int GradeScore;
    [SerializeField] public Sprite GradeSprite;
}
